
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



builder.Services.AddDbContext<UserScoreDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"));
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
