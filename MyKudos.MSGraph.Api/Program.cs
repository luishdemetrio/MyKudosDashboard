using MyKudos.MSGraph.Api.Grpc;
using MyKudos.MSGraph.Api.Interfaces;
using MyKudos.MSGraph.Api.Services;
using MyKudos.MSGraph.gRPC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IGraphService, GraphService>();

builder.Services.AddGrpc(c => c.EnableDetailedErrors = true);


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


app.MapGrpcService<GraphGrpc>();

app.Run();
