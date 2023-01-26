using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IGraphService, GraphService>();
builder.Services.AddScoped<IRecognitionService, RecognitionService>();
builder.Services.AddScoped<IKudosService, KudosService>();
builder.Services.AddScoped<IAgentNotificationService, AgentNotificationService>();

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
