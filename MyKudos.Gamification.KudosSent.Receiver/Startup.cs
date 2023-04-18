using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Communication.Helper.Services;
using MyKudos.Gamification.Receiver;
using MyKudos.Gamification.Receiver.Interfaces;
using MyKudos.Gamification.Receiver.MessageSenders;
using MyKudos.Gamification.Receiver.Services;
using MyKudos.MessageSender.Interfaces;
using MyKudos.Queue.Services;

[assembly: FunctionsStartup(typeof(Startup))]

namespace MyKudos.Gamification.Receiver;

public class Startup : FunctionsStartup
{

    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddSingleton<IUserScoreService, UserScoreService>();

        // Send Topic 
        builder.Services.AddSingleton<IScoreMessageSender, ScoreMessageSender>();

        // Add configuration to the builder
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("host.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        // Register the IConfiguration instance in the container
        builder.Services.AddSingleton<IConfiguration>(configuration);


        builder.Services.AddSingleton<IRestClientHelper>(t =>
                        new RestClientHelper(
                           new RestServiceToken(
                            clientId: configuration["ClientId"],
                            clientSecret: configuration["ClientSecret"],
                            tenantId: configuration["TenantId"],
                            exposedAPI: configuration["ExposedApi"]
                        )
                        ));



        builder.Services.AddSingleton<IMessageSender>(t =>
                        new ServiceBusMessageSender(
                            serviceBusConnectionString: configuration["KudosServiceBus_ConnectionString"]
                        ));

    }

   

}
