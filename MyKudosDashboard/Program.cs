using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Identity.Client;
using Microsoft.TeamsFx.Helper;
using Microsoft.TeamsFx;
using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Interop.TeamsSDK;
using MyKudosDashboard.Services;
using MyKudosDashboard.Views;
using MyKudos.Kudos.Token.Interfaces;
using MyKudos.Kudos.Token.Services;
//using Radzen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
//builder.Services.AddRazorPages(options =>
//{
//    options.Conventions.AuthorizeFolder("/tab");

//});




builder.Services.AddServerSideBlazor();

builder.Services.AddTeamsFx(builder.Configuration.GetSection("TeamsFx"));
builder.Services.AddScoped<MicrosoftTeams>();

builder.Services.AddControllers();
builder.Services.AddHttpClient("WebClient", client => client.Timeout = TimeSpan.FromSeconds(600));
builder.Services.AddHttpContextAccessor();

builder.Services.AddFluentUIComponents();

//Radzen
//builder.Services.AddScoped<DialogService>();
//builder.Services.AddScoped<NotificationService>();
//builder.Services.AddScoped<TooltipService>();
//builder.Services.AddScoped<ContextMenuService>();

//Views
builder.Services.AddScoped<ISendKudosView, SendKudosView>();
builder.Services.AddScoped<IWelcomeView, WelcomeView>();
builder.Services.AddScoped<IKudosListView, KudosListView>();
builder.Services.AddScoped<IUserProfileScoreView, UserProfileScoreView>();  
builder.Services.AddScoped<ITopContributorsView, TopContributorsView>();

//Services

builder.Services.AddSingleton<IGatewayService, GatewayService>();

var config = builder.Configuration;

builder.Services.AddSingleton<IRestServiceToken>(t =>
                new RestServiceToken(
                    clientId: config["ClientId"],
                    clientSecret: config["ClientSecret"],
                    tenantId: config["TenantId"],
                    exposedAPI: config["ExposedApi"]
                ));


//// Add MS GRAPH services to the container.
//builder.Services
//    // Use Web API authentication (default JWT bearer token scheme)
//    .AddMicrosoftIdentityWebApiAuthentication(builder.Configuration)
//    // Enable token acquisition via on-behalf-of flow
//    .EnableTokenAcquisitionToCallDownstreamApi()
//    // Add authenticated Graph client via dependency injection
//    .AddMicrosoftGraph(builder.Configuration.GetSection("Graph"))
//    // Use in-memory token cache
//    // See https://github.com/AzureAD/microsoft-identity-web/wiki/token-cache-serialization
//    .AddInMemoryTokenCaches();

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

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
    endpoints.MapControllers();
});

app.Run();

