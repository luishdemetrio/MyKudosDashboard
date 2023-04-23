using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace MyKudosDashboard.MessageSender;

public class ServiceBusSubscriberHelper
{

    private ServiceBusClient _serviceBusClient;
    private ServiceBusProcessor _serviceBusProcessor;

    private readonly string _serviceBusConnectionString;

    private ILogger _logger;

    public ServiceBusSubscriberHelper(IConfiguration configuration, ILogger logger)
    {
        _serviceBusConnectionString = configuration["KudosServiceBus_ConnectionString"];

        _serviceBusClient = new ServiceBusClient(_serviceBusConnectionString);

        _logger = logger;
    }

    public void ServiceBusProcessor(ServiceBusProcessorConfig config)
    {
        CreateASubscriberfItDoesntExistAsync(config.DashboardName, config.SubscriptionName).ContinueWith(t =>
        {

            _serviceBusProcessor = _serviceBusClient.CreateProcessor(config.DashboardName, config.SubscriptionName);

            _serviceBusProcessor.ProcessMessageAsync += config.MessageProcessor;

            _serviceBusProcessor.ProcessErrorAsync += ServiceBusProcessor_ProcessErrorAsync;

            _serviceBusProcessor.StartProcessingAsync();

        });
    }

    private async Task ServiceBusProcessor_ProcessErrorAsync(ProcessErrorEventArgs arg)
    {
                _logger.LogError($"ServiceBusSubscriberHelper: Error processing message: {arg.Exception}");
    }

    public async Task CreateASubscriberfItDoesntExistAsync(string topicName, string subscriptionName)
    {

        var serviceBusAdminClient = new ServiceBusAdministrationClient(_serviceBusConnectionString);

        
        if (await serviceBusAdminClient.SubscriptionExistsAsync(topicName, subscriptionName))
        {

            await serviceBusAdminClient.DeleteSubscriptionAsync(topicName, subscriptionName);

            //create a subscription for the user

            var options = new CreateSubscriptionOptions(topicName, subscriptionName)
            {
                AutoDeleteOnIdle = TimeSpan.FromHours(12),
                LockDuration = TimeSpan.FromMinutes(2),
                MaxDeliveryCount = 10,
                DefaultMessageTimeToLive = TimeSpan.FromMinutes(2)

            };

            await serviceBusAdminClient.CreateSubscriptionAsync(options);

        }

    }

}
