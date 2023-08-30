using Microsoft.EntityFrameworkCore;
using SuperKudos.KudosCatalog.Data.Context;
using SuperKudos.KudosCatalog.Data.Repository;
using SuperKudos.KudosCatalog.Domain.Interfaces;
using MyKudos.MSGraph.Api.Interfaces;
using MyKudos.MSGraph.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IGraphService, GraphService>();

builder.Services.AddTransient<IUserProfileRepository, UserProfileRepository>();

builder.Services.AddDbContext<KudosDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"));

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


//app.MapGrpcService<GraphGrpc>();

app.Run();
