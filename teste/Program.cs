using Microsoft.Fast.Components.FluentUI;
using teste;
using teste.Interop.TeamsSDK;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var config = builder.Configuration.Get<ConfigOptions>();
builder.Services.AddTeamsFx(config.TeamsFx.Authentication);
builder.Services.AddScoped<MicrosoftTeams>();

builder.Services.AddControllers();
builder.Services.AddHttpClient("WebClient", client => client.Timeout = TimeSpan.FromSeconds(600));
builder.Services.AddHttpContextAccessor();

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

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
    endpoints.MapControllers();
});

app.Run();

