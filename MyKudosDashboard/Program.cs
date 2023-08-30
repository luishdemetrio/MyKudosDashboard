using Microsoft.AspNetCore.Localization;
using Microsoft.Fast.Components.FluentUI;
using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Communication.Helper.Services;
using SuperKudos.Aggregator.Domain.Models;
using MyKudosDashboard.EventHub;
using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Interop.TeamsSDK;
using MyKudosDashboard.Services;
using MyKudosDashboard.Views;
using System.Globalization;
using Dapr.Client;
using Dapr.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

var daprClient = new DaprClientBuilder().Build();

//builder.WebHost.ConfigureAppConfiguration(config =>
//{

//    config.AddDaprSecretStore("secretstore-superkudos", daprClient);

//});

Console.WriteLine($"mandalorian: {builder.Configuration["ClientId"]}");



builder.Services.AddDaprClient();



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

//Views
builder.Services.AddScoped<ISendKudosView, SendKudosView>();
builder.Services.AddScoped<IWelcomeView, WelcomeView>();
builder.Services.AddScoped<IKudosListView, KudosListView>();
builder.Services.AddScoped<IUserProfileScoreView, UserProfileScoreView>();  
builder.Services.AddScoped<ITopContributorsView, TopContributorsView>();
builder.Services.AddScoped<IKudosTabView, KudosTabView>();
builder.Services.AddScoped<ICommentsView, CommentsView>();
builder.Services.AddScoped<IReplyView, ReplyView>();

//Services

builder.Services.AddScoped<ICommentsGateway, CommentsGateway>();
builder.Services.AddScoped<IGamificationGateway, GamificationGateway>();
builder.Services.AddScoped<IKudosGateway, GatewayService>();
builder.Services.AddScoped<IRecognitionGateway, RecognitionGateway>();
builder.Services.AddScoped<IUserGateway, UserGateway>();

//builder.Services.AddSingleton<IEventGridKudosReceived, EventGridKudosReceived>();
//builder.Services.AddSingleton<IEventGridUserPointsReceived, EventGridUserPointsReceived>();

//Event Hub
builder.Services.AddSingleton<IEventHubReceived<UserPointScore>, EventHubUserPointsReceived>();

builder.Services.AddSingleton<IEventHubCommentDeleted, EventHubCommentDeleted>();
builder.Services.AddSingleton<IEventHubCommentSent, EventHubCommentSent>();
builder.Services.AddSingleton<IEventHubKudosSent, EventHubKudosSent>();
builder.Services.AddSingleton<IEventHubLikeSent, EventHubLikeSent>();
builder.Services.AddSingleton<IEventHubUndoLike, EventHubUndoLike>();
builder.Services.AddSingleton<IEventHubUserPointsReceived, EventHubUserPointsReceived>();


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

//if (!string.IsNullOrEmpty(builder.Configuration.GetConnectionString("APPLICATIONINSIGHTS_CONNECTION_STRING")))
//{
//    builder.Logging.AddApplicationInsights(
//            configureTelemetryConfiguration: (config) =>
//                config.ConnectionString = builder.Configuration.GetConnectionString("APPLICATIONINSIGHTS_CONNECTION_STRING"),
//                configureApplicationInsightsLoggerOptions: (options) => { }
//        );

//}

//builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>("superkudos", LogLevel.Trace);


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

var supportedCultures = new[] { new CultureInfo("pt-BR") };

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(selectedCulture),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

app.UseRequestLocalization(localizationOptions);



app.UseAuthentication();
app.UseAuthorization();

 app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
    
});

app.Run();

