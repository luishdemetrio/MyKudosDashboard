using Microsoft.AspNetCore.Localization;
using Microsoft.Bot.Builder.Dialogs.Memory.Scopes;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Fast.Components.FluentUI;
using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Communication.Helper.Services;
using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.Common;
using MyKudosDashboard.EventHub;
using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Interfaces.Aggregator;
using MyKudosDashboard.Interop.TeamsSDK;
using MyKudosDashboard.Services;
using MyKudosDashboard.Views;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddServerSideBlazor();

builder.Services.AddTeamsFx(builder.Configuration.GetSection("TeamsFx"));
builder.Services.AddScoped<MicrosoftTeams>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});


builder.Services.AddHttpClient("WebClient", client => client.Timeout = TimeSpan.FromSeconds(600));
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IImageHelper, ImageHelper>();
builder.Services.AddScoped<KudosCommonVariables>();

//Views
builder.Services.AddScoped<ISendKudosView, SendKudosView>();
builder.Services.AddScoped<IWelcomeView, WelcomeView>();

//this one needs to be transient since it is used by user control having dedicated lists on it
builder.Services.AddTransient<IKudosListView, KudosListView>();

builder.Services.AddScoped<IUserProfileScoreView, UserProfileScoreView>();  
builder.Services.AddScoped<ITopContributorsView, TopContributorsView>();
builder.Services.AddScoped<IKudosTabView, KudosTabView>();
builder.Services.AddScoped<ICommentsView, CommentsView>();
builder.Services.AddScoped<IReplyView, ReplyView>();
builder.Services.AddScoped<IRewriteView, RewriteView>();

//Services

builder.Services.AddScoped<ICommentsGateway, CommentsGateway>();
builder.Services.AddScoped<IGamificationGateway, GamificationGateway>();
builder.Services.AddScoped<IKudosGateway, GatewayService>();
builder.Services.AddScoped<IRecognitionGateway, RecognitionGateway>();
builder.Services.AddScoped<IUserGateway, UserGateway>();

builder.Services.AddScoped<IRecognitionGroupAggregator, RecognitionGroupAggregator>();
builder.Services.AddScoped<IRecognitionAggregator, RecognitionAggregator>();
builder.Services.AddScoped<IAdminUserAggregador, AdminUserAggregador>();
builder.Services.AddScoped<IScorePointsAggregator, ScorePointsAggregator>();

//Event Hub
builder.Services.AddSingleton<IEventHubReceived<UserPointScore>, EventHubUserPointsReceived>();

builder.Services.AddSingleton<IEventHubCommentDeleted, EventHubCommentDeleted>();
builder.Services.AddSingleton<IEventHubCommentSent, EventHubCommentSent>();
builder.Services.AddSingleton<IEventHubKudosSent, EventHubKudosSent>();
builder.Services.AddSingleton<IEventHubLikeSent, EventHubLikeSent>();
builder.Services.AddSingleton<IEventHubUndoLike, EventHubUndoLike>();
builder.Services.AddSingleton<IEventHubUserPointsReceived, EventHubUserPointsReceived>();
builder.Services.AddSingleton<IEventHubKudosDeleted, EventHubKudosDeleted>();
builder.Services.AddSingleton<IEventHubKudosUpdated, EventHubKudosUpdated>();


var config = builder.Configuration;



builder.Services.AddScoped<IRestClientHelper>(t =>
                new RestClientHelper(
                   new RestServiceToken(
                    clientId: config["ClientId"],
                    clientSecret: config["ClientSecret"],
                    tenantId: config["TenantId"],
                    exposedAPI: config["ExposedApi"]
                )
                ));


builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");


builder.Services.AddFluentUIComponents();
//LibraryConfiguration configUI = new(Microsoft.Fast.Components.FluentUI.ConfigurationGenerator.GetIconConfiguration(), Microsoft.Fast.Components.FluentUI.ConfigurationGenerator.GetEmojiConfiguration());
//builder.Services.AddFluentUIComponents(configUI);

if (!string.IsNullOrEmpty(builder.Configuration.GetConnectionString("APPLICATIONINSIGHTS_CONNECTION_STRING")))
{
    builder.Logging.AddApplicationInsights(
            configureTelemetryConfiguration: (config) =>
                config.ConnectionString = builder.Configuration.GetConnectionString("APPLICATIONINSIGHTS_CONNECTION_STRING"),
                configureApplicationInsightsLoggerOptions: (options) => { }
        );

}

builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>("superkudos", LogLevel.Trace);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

var selectedCulture = config["SelectedCulture"];

var supportedCultures = new[] { new CultureInfo("pt-br"), new CultureInfo("en-us") };

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(selectedCulture),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

app.UseRequestLocalization(localizationOptions);

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);


app.UseAuthentication();
app.UseAuthorization();

 app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
    
});

app.Run();

