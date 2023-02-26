using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Services;
using MyKudos.Gateway.Services.Rest;
using MyKudos.Kudos.Token.Interfaces;
using MyKudos.Kudos.Token.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IGraphService, GraphServiceRest>();
builder.Services.AddSingleton<IRecognitionService, RecognitionServiceRest>();
builder.Services.AddSingleton<IKudosService, KudosServiceRest>();
builder.Services.AddSingleton<IAgentNotificationService, AgentNotificationService>();
builder.Services.AddSingleton<IGamificationService, GamificationService>();

var config = builder.Configuration;

builder.Services.AddSingleton<IRestServiceToken>( t =>
                new RestServiceToken(
                    clientId: config["ClientId"],
                    clientSecret: config["ClientSecret"],
                    tenantId: config["TenantId"],
                    exposedAPI: config["ExposedApi"]
                ));

builder.Services.AddSingleton<IKudosQueue, KudosQueue>();

builder.Services.AddSwaggerGen(c =>
{
   
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();


}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
