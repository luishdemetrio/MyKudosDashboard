

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyKudos.Kudos.Token.Interfaces;
using MyKudos.Kudos.Token.Services;
using MyKudos.Test.Console;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.development.json", optional: false, reloadOnChange: true)
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .Build();


        var services = new ServiceCollection();

        services.AddSingleton(configuration);

      

        // Add more services here

        // Build the service provider
        var serviceProvider = services.BuildServiceProvider();


        var gatewayService = new GatewayService(
                         new RestServiceToken(
                            clientId: configuration["ClientId"],
                            clientSecret: configuration["ClientSecret"],
                            tenantId: configuration["TenantId"],
                            exposedAPI: configuration["ExposedApi"]
                        ));


        var succeed = await gatewayService.GetRecognitionsAsync().ConfigureAwait(false);

        await Console.Out.WriteLineAsync(   succeed.ToString());


    }
}