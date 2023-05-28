using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using System.Security.Cryptography;
using System.Text;

namespace MyKudosDashboard.MessageSender;

public class ServiceBusSubscriberHelper
{

    private ServiceBusClient _serviceBusClient;
    private ServiceBusProcessor _serviceBusProcessor;

    private readonly string _serviceBusConnectionString;

    private ILogger _logger;

    private IConfiguration _configuration;
    public ServiceBusSubscriberHelper(IConfiguration configuration, ILogger logger)
    {
        _serviceBusConnectionString = configuration["KudosServiceBus_ConnectionString"];

        _serviceBusClient = new ServiceBusClient(_serviceBusConnectionString);

        _logger = logger;

        _configuration = configuration;
    }

    public void ServiceBusProcessor(ServiceBusProcessorConfig config)
    {

        CreateASubscriberfItDoesntExistAsync(config.DashboardName, config.SubscriptionName).ContinueWith(t =>
        {

            _serviceBusProcessor = _serviceBusClient.CreateProcessor(config.DashboardName, config.SubscriptionName);

            _serviceBusProcessor.ProcessMessageAsync += config.MessageProcessor;

            _serviceBusProcessor.ProcessErrorAsync += async arg => {
                _logger.LogError($"ServiceBusSubscriberHelper: Error processing message: {arg.Exception}");

                await Task.CompletedTask;
            };

            _serviceBusProcessor.StartProcessingAsync();

        });


    }

    public static string GetHash(string input)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }


    public string GetInstanceId(string subscriptionName )
    {

        string instanceId = _configuration["WEBSITE_INSTANCE_ID"];
        string hash = string.Empty;

        if (!string.IsNullOrEmpty(instanceId))
        {
            hash = GetHash(instanceId);
        }
        subscriptionName += hash;

        if (subscriptionName.Length > 50)
        {
            subscriptionName = subscriptionName.Substring(0, 50);
        }

        _logger.LogInformation(subscriptionName);

        return subscriptionName;
    }


    public async Task CreateASubscriberfItDoesntExistAsync(string topicName, string subscriptionName)
    {

        var serviceBusAdminClient = new ServiceBusAdministrationClient(_serviceBusConnectionString);


        if (!await serviceBusAdminClient.SubscriptionExistsAsync(topicName, subscriptionName))
        {


            //create a subscription for the user

            var options = new CreateSubscriptionOptions(topicName, subscriptionName)
            {
                AutoDeleteOnIdle = TimeSpan.FromHours(12),
                LockDuration = TimeSpan.FromMinutes(5),
                MaxDeliveryCount = 10,
                DefaultMessageTimeToLive = TimeSpan.FromMinutes(10)
                

            };

            await serviceBusAdminClient.CreateSubscriptionAsync(options);

        }

    }

}
