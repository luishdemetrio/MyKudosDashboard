using Microsoft.EntityFrameworkCore;
using MyKudos.Domain.Core.Bus;
//using MyKudos.Kudos.Api.Grpc;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.App.Services;
using MyKudos.Kudos.Data.Context;
using MyKudos.Kudos.Data.Repository;
using MyKudos.Kudos.Domain.EventHandlers;
using MyKudos.Kudos.Domain.Events;
using MyKudos.Kudos.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

//builder.Services.AddMediatR(typeof(Program));

builder.Services.AddGrpc(c => c.EnableDetailedErrors = true);

builder.Services.AddScoped<IKudosService, KudosService>();
builder.Services.AddScoped<IKudosRepository, KudosRepository>();

var config = builder.Configuration.GetSection("CosmosDb");

builder.Services.AddScoped<KudosDbContext>(_ =>
{
    var options = new DbContextOptionsBuilder<KudosDbContext>()
      .UseCosmos(
              config["AccountEndPoint"],
              config["AccountKey"],
              config["DatabaseName"])
      .Options;

    return new KudosDbContext(options);
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

//ConfigureEventBus(app);

//app.MapGrpcService<KudosGrpc>();

app.Run();


void ConfigureEventBus(IApplicationBuilder app)
{
    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

    eventBus.Subscribe<SendKudosCreatedEvent, SendKudosEventHandler>();
}

