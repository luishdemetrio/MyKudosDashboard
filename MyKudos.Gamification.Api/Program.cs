using Microsoft.EntityFrameworkCore;
using MyKudos.Gamification.App.Interfaces;
using MyKudos.Gamification.App.Services;
using MyKudos.Gamification.Data.Context;
using MyKudos.Gamification.Data.Repository;
using MyKudos.Gamification.Domain.Intefaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var config = builder.Configuration.GetSection("CosmosDb");

builder.Services.AddScoped<UserScoreDbContext>(_ =>
{
    var options = new DbContextOptionsBuilder<UserScoreDbContext>()
      .UseCosmos(
              config["AccountEndPoint"],
              config["AccountKey"],
              config["DatabaseName"])
      .Options;

    return new UserScoreDbContext(options);
});

builder.Services.AddScoped<IUserScoreService, UserScoreService>();
builder.Services.AddScoped<IUserScoreRepository, UserScoreRepository>();

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
