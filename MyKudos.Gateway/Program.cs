using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Services;
using MyKudos.Gateway.Services.Rest;

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
builder.Services.AddSingleton<IRestServiceToken, RestServiceToken>();

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
