using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyKudos.Recognition.App.Interfaces;
using MyKudos.Recognition.App.Services;
using MyKudos.Recognition.Data.Context;
using MyKudos.Recognition.Data.Repository;
using MyKudos.Domain.Core.Bus;
using MyKudos.Infra.Bus;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.App.Services;
using MyKudos.Kudos.Data.Context;
using MyKudos.Kudos.Data.Repository;
using MyKudos.Kudos.Domain.EventHandlers;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Recognition.Domain.Interfaces;
using MyKudos.Kudos.Domain.Commands;
using MyKudos.Kudos.Domain.CommandHandlers;
using MyKudos.Kudos.Domain.Services;

namespace MyKudos.Infra.IoC;

public class DependencyContainer
{

    public static void RegisterServices(IServiceCollection services)
    {
        //Domain Bus
        services.AddSingleton<IEventBus, RabbitMQBus>(sp =>
        {
            var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
            return new RabbitMQBus(sp.GetService<IMediator>(), scopeFactory);
        });

        //Domain Banking Commands
        services.AddTransient<IRequestHandler<CreateSendKudosCommand, bool>, SendKudosCommandHandler>();

        //Application services
        services.AddTransient<IRecognitionService, RecognitionService>();
        services.AddTransient<IKudosService, KudosService>();
        services.AddTransient<IAgentNotificationService, AgentNotificationService>();

        //Data
        services.AddTransient<RecognitionDbContext>(_ =>
        {
            var options = new DbContextOptionsBuilder<RecognitionDbContext>()
              .UseCosmos(
                      "https://mykudos.documents.azure.com:443/",
                      "pPT5EVtJyAh0Lk4N7ywHk2ZgPTSepeH6YvbUYw2R6msjLeCQLHMs1KfhOE5xPdoHUQVR3vMFiXvmACDbOWmCqA==",
                      databaseName: "dashboard-db")
              .Options;

            return new RecognitionDbContext(options);
        });

        services.AddTransient<KudosDbContext>(_ =>
        {
            var options = new DbContextOptionsBuilder<KudosDbContext>()
              .UseCosmos(
                      "https://mykudos.documents.azure.com:443/",
                      "pPT5EVtJyAh0Lk4N7ywHk2ZgPTSepeH6YvbUYw2R6msjLeCQLHMs1KfhOE5xPdoHUQVR3vMFiXvmACDbOWmCqA==",
                      databaseName: "kudos-db")
              .Options;

            return new KudosDbContext(options);
        });

        services.AddTransient<IRecognitionRepository, RecognitionRepository>();
        services.AddTransient<IKudosRepository, KudosRepository>();

        //Subscriptions
        services.AddTransient<SendKudosEventHandler>();

        //Domain Events
        services.AddTransient<IEventHandler<MyKudos.Kudos.Domain.Events.SendKudosCreatedEvent>, MyKudos.Kudos.Domain.EventHandlers.SendKudosEventHandler>();




    }

}