using Microsoft.EntityFrameworkCore;
using MyKudos.Recognition.Data.Context;
using MyKudos.Recognition.Api.Grpc;
using MyKudos.Recognition.App.Interfaces;
using MyKudos.Recognition.App.Services;
using MyKudos.Recognition.Data.Repository;
using MyKudos.Recognition.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

////// Add services to the container.
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));


builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IRecognitionService, RecognitionService>();
builder.Services.AddScoped<IRecognitionRepository, RecognitionRepository>();

var config = builder.Configuration.GetSection("CosmosDb");

builder.Services.AddDbContext<RecognitionDbContext>(options =>
{
    options.UseCosmos(
              config["AccountEndPoint"],
              config["AccountKey"],
              config["DatabaseName"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//app.MapGrpcService<RecognitionGrpc>();

app.Run();
