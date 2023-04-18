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

builder.Services.AddFluentUIComponents();


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

builder.Services.AddSingleton<ICommentsGateway, CommentsGateway>();
builder.Services.AddSingleton<IGamificationGateway, GamificationGateway>();
builder.Services.AddSingleton<IKudosGateway, GatewayService>();
builder.Services.AddSingleton<IRecognitionGateway, RecognitionGateway>();
builder.Services.AddSingleton<IUserGateway, UserGateway>();



var config = builder.Configuration;

builder.Services.AddSingleton<IRestClientHelper>(t =>
                new RestClientHelper(
                   new RestServiceToken(
                    clientId: config["ClientId"],
                    clientSecret: config["ClientSecret"],
                    tenantId: config["TenantId"],
                    exposedAPI: config["ExposedApi"]
                )
                ));


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

app.UseCors(builder => builder.WithOrigins("https://localhost:44302")
                                .AllowAnyMethod()
                                .AllowAnyHeader());
app.Run();

