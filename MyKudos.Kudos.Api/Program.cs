using MediatR;
using Microsoft.EntityFrameworkCore;
using MyKudos.Domain.Core.Bus;
using MyKudos.Infra.IoC;
using MyKudos.Kudos.Api.Grpc;
using MyKudos.Kudos.Data.Context;
using MyKudos.Kudos.Domain.EventHandlers;
using MyKudos.Kudos.Domain.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

builder.Services.AddMediatR(typeof(Program));

builder.Services.AddGrpc(c => c.EnableDetailedErrors = true);

DependencyContainer.RegisterServices(builder.Services);


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

ConfigureEventBus(app);

app.MapGrpcService<KudosGrpc>();

app.Run();


void ConfigureEventBus(IApplicationBuilder app)
{
    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

    eventBus.Subscribe<SendKudosCreatedEvent, SendKudosEventHandler>();
}

