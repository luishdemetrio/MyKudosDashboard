using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Models;
using Newtonsoft.Json;

namespace MyKudos.Gateway.Services;

public class KudosQueue : IKudosQueue
{
    private static string _connectionString = string.Empty;
    private static string _topicName = string.Empty;

    public KudosQueue(IConfiguration configuration)
    {
        _connectionString = configuration["KudosServiceBus_ConnectionString"];
        _topicName = configuration["KudosServiceBus_TopicName"];
    }

    public async Task SendAsync(KudosNotification kudos)
    {

        //create an admin client to manage artifacts

        var serviceBusAdminClient = new ServiceBusAdministrationClient(_connectionString);

        //create a topic if it doesnt exist

        if (! await serviceBusAdminClient.TopicExistsAsync(_topicName))
        {
            await serviceBusAdminClient.CreateTopicAsync(_topicName);
        }

        //create a temp subscription for the user

        if (!await serviceBusAdminClient.SubscriptionExistsAsync(_topicName, "notification"))
        {
            var options = new CreateSubscriptionOptions(_topicName, "notification")
            {
                AutoDeleteOnIdle = TimeSpan.FromHours(1)
            };

            await serviceBusAdminClient.CreateSubscriptionAsync(options);
        }

        var client = new ServiceBusClient(_connectionString);

        var sender = client.CreateSender(_topicName);

        var message = new ServiceBusMessage(JsonConvert.SerializeObject(kudos));
        await sender.SendMessageAsync(message);

        await sender.CloseAsync();

    }
    
}
