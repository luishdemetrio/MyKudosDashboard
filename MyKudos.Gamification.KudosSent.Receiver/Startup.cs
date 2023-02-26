using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MyKudos.Gamification.Receiver;
using MyKudos.Gamification.Receiver.Interfaces;
using MyKudos.Gamification.Receiver.Services;


[assembly: FunctionsStartup(typeof(Startup))]

namespace MyKudos.Gamification.Receiver;

public class Startup : FunctionsStartup
{

    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddSingleton<IUserScoreService, UserScoreService>();

        builder.Services.AddSingleton<IRestServiceToken, RestServiceToken>();
    }

   

}
