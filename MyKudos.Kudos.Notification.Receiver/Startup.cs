using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MyKudos.Kudos.Notification.Receiver.Interfaces;

[assembly: FunctionsStartup(typeof(MyKudos.Kudos.Notification.Receiver.Startup))]

namespace MyKudos.Kudos.Notification.Receiver;

public class Startup : FunctionsStartup
{

    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddSingleton<IAgentNotificationService, AgentNotificationService>();
        builder.Services.AddSingleton<IRestServiceToken, RestServiceToken>();
        
    }


}
