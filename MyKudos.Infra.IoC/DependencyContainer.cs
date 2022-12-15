using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyKudos.Dashboard.App.Interface;
using MyKudos.Dashboard.App.Services;
using MyKudos.Dashboard.Data.Context;
using MyKudos.Dashboard.Data.Repository;
using MyKudos.Dashboard.Domain.CommandHandlers;
using MyKudos.Dashboard.Domain.Commands;
using MyKudos.Dashboard.Domain.Events;
using MyKudos.Dashboard.Domain.Interfaces;
using MyKudos.Domain.Core.Bus;
using MyKudos.Infra.Bus;

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

        //Application services
        services.AddTransient<IRecognitionService, RecognitionService>();

        //Data

        services.AddTransient<RecognitionDbContext>(_ =>
        {
            var options = new DbContextOptionsBuilder<RecognitionDbContext>()
              .UseCosmos(
                      "https://mykudos.documents.azure.com:443/",
                      "pPT5EVtJyAh0Lk4N7ywHk2ZgPTSepeH6YvbUYw2R6msjLeCQLHMs1KfhOE5xPdoHUQVR3vMFiXvmACDbOWmCqA==",
                      databaseName: "kudosdb")
              .Options;

            return new RecognitionDbContext(options);
        });

        services.AddTransient<IRecognitionRepository, RecognitionRepository>();

        //Subscriptions
        

        //Domain Events
        


        //Domain Banking Commands
        services.AddTransient<IRequestHandler<CreateSendKudosCommand, bool>, SendKudosCommandHandler>();

        //Application Services
        services.AddTransient<IKudosService, KudosService>();
        

    }

}