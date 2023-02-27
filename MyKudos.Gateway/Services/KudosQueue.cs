using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Models;
using Newtonsoft.Json;

namespace MyKudos.Gateway.Services;

public class KudosQueue : IKudosQueue
{
    private static string _connectionString = string.Empty;
    private static string _notificationTopicName = string.Empty;

    private static string _gamificationKudosSentTopicName = string.Empty;
    private static string _gamificationKudosReceivedTopicName = string.Empty;

    private static string _gamificationLikeSentTopicName = string.Empty;
    private static string _gamificationLikeReceivedTopicName = string.Empty;

    public KudosQueue(IConfiguration configuration)
    {
        _connectionString = configuration["KudosServiceBus_ConnectionString"];
        _notificationTopicName = configuration["KudosServiceBus_TopicName"];
        
        _gamificationKudosSentTopicName = configuration["KudosServiceBus_GamificationKudosSentTopicName"];
        _gamificationKudosReceivedTopicName = configuration["KudosServiceBus_GamificationKudosReceivedTopicName"];

        _gamificationLikeSentTopicName = configuration["KudosServiceBus_GamificationLikeSentTopicName"];
        _gamificationLikeReceivedTopicName = configuration["KudosServiceBus_GamificationLikeReceivedTopicName"];
    }

    public async Task SendKudosAsync(KudosNotification kudos)
    {

        //create an admin client to manage artifacts

        var serviceBusAdminClient = new ServiceBusAdministrationClient(_connectionString);

        await SendTopic(kudos, serviceBusAdminClient, _notificationTopicName, "notification");

        await SendTopic(kudos.From.Id, serviceBusAdminClient, _gamificationKudosSentTopicName, "notification");

        await SendTopic(kudos.To.Id, serviceBusAdminClient, _gamificationKudosReceivedTopicName, "notification") ;

        

    }

    private static async Task SendTopic(object queueMessage, ServiceBusAdministrationClient serviceBusAdminClient,
                                        string topic, string subscriptionName)
    {
        
        //create a topic if it doesnt exist

        if (!await serviceBusAdminClient.TopicExistsAsync(topic))
        {
            await serviceBusAdminClient.CreateTopicAsync(topic);
        }

        //create a temp subscription for the user

        if (!await serviceBusAdminClient.SubscriptionExistsAsync(topic, subscriptionName))
        {
            var options = new CreateSubscriptionOptions(topic, subscriptionName)
            {
                AutoDeleteOnIdle = TimeSpan.FromHours(1)
            };

            await serviceBusAdminClient.CreateSubscriptionAsync(options);
        }

        var client = new ServiceBusClient(_connectionString);

        var sender = client.CreateSender(topic);

        var message = new ServiceBusMessage(JsonConvert.SerializeObject(queueMessage));

        await sender.SendMessageAsync(message);

        await sender.CloseAsync();
    }

    public async Task SendLikeAsync(LikeGateway like )
    {
        var serviceBusAdminClient = new ServiceBusAdministrationClient(_connectionString);

        
        await SendTopic(like.FromPersonId, serviceBusAdminClient, _gamificationLikeSentTopicName, "notification");

        await SendTopic(like.ToPersonId, serviceBusAdminClient, _gamificationLikeReceivedTopicName, "notification");
    }
}
