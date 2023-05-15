using Microsoft.Fast.Components.FluentUI;
using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Communication.Helper.Services;
using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Interop.TeamsSDK;
using MyKudosDashboard.Services;
using MyKudosDashboard.Views;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddServerSideBlazor();

builder.Services.AddTeamsFx(builder.Configuration.GetSection("TeamsFx"));
builder.Services.AddScoped<MicrosoftTeams>();

builder.Services.AddControllers();
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

LibraryConfiguration configUI = new(Microsoft.Fast.Components.FluentUI.ConfigurationGenerator.GetIconConfiguration(), Microsoft.Fast.Components.FluentUI.ConfigurationGenerator.GetEmojiConfiguration());
builder.Services.AddFluentUIComponents(configUI);



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

var supportedCultures = new[] { "pt-BR" };

var localizationOptions = new RequestLocalizationOptions()
    //.AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);



app.UseAuthentication();
app.UseAuthorization();

 app.UseEndpoints(endpoints =>
{
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
    endpoints.MapControllers();
});

app.Run();

