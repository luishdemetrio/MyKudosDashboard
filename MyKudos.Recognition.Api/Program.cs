using Microsoft.EntityFrameworkCore;
using MyKudos.Recognition.Data.Context;
using MyKudos.Recognition.Api.Grpc;
using MyKudos.Recognition.App.Interfaces;
using MyKudos.Recognition.App.Services;
using MyKudos.Recognition.Data.Repository;
using MyKudos.Recognition.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddMediatR(typeof(Program));

builder.Services.AddGrpc(c => c.EnableDetailedErrors = true);

//DependencyContainer.RegisterServices(builder.Services);

builder.Services.AddScoped<IRecognitionService, RecognitionService>();
builder.Services.AddScoped<IRecognitionRepository, RecognitionRepository>();

//string connectionString = builder.Configuration.GetConnectionString("AppConfig");

//// Load configuration from Azure App Configuration
//builder.Configuration.AddAzureAppConfiguration(connectionString);


var config = builder.Configuration.GetSection("CosmosDb");

builder.Services.AddDbContext<RecognitionDbContext>(options =>
{
    options.UseCosmos(
              config["AccountEndPoint"],
              config["AccountKey"],
              config["DatabaseName"]);
});

//builder.Services.AddScoped<RecognitionDbContext>(_ =>
//{
//    var options = new DbContextOptionsBuilder<RecognitionDbContext>()
//      .UseCosmos(
//              config["AccountEndPoint"],
//              config["AccountKey"],
//              config["DatabaseName"])
//      .Options;

//    return new RecognitionDbContext(options);
//});

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

app.MapGrpcService<RecognitionGrpc>();

app.Run();
