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


    private static string _gamificationMessageSentTopicName = string.Empty;
    private static string _gamificationMessageReceivedTopicName = string.Empty;
    private static string _gamificationMessageDeletedTopicName = string.Empty;

    public KudosQueue(IConfiguration configuration)
    {
        _connectionString = configuration["KudosServiceBus_ConnectionString"];
        _notificationTopicName = configuration["KudosServiceBus_TopicName"];
        
        _gamificationKudosSentTopicName = configuration["KudosServiceBus_GamificationKudosSentTopicName"];
        _gamificationKudosReceivedTopicName = configuration["KudosServiceBus_GamificationKudosReceivedTopicName"];

        _gamificationLikeSentTopicName = configuration["KudosServiceBus_GamificationLikeSentTopicName"];
        _gamificationLikeReceivedTopicName = configuration["KudosServiceBus_GamificationLikeReceivedTopicName"];

        _gamificationMessageSentTopicName = configuration["KudosServiceBus_GamificationMessageSentTopicName"];
        _gamificationMessageReceivedTopicName = configuration["KudosServiceBus_GamificationMessageReceivedTopicName"];
        _gamificationMessageDeletedTopicName = configuration["KudosServiceBus_GamificationMessageDeletedTopicName"];
    }

    public async Task SendKudosAsync(KudosNotification kudos)
    {

        //create an admin client to manage artifacts

        var serviceBusAdminClient = new ServiceBusAdministrationClient(_connectionString);

        //send notification via Bot
        await SendTopic(kudos, serviceBusAdminClient, _notificationTopicName, "notification");

        //gamification
        await SendTopic(kudos.From.Id, serviceBusAdminClient, _gamificationKudosSentTopicName, "notification");
        await SendTopic(kudos.To.Id, serviceBusAdminClient, _gamificationKudosReceivedTopicName, "notification") ;

        //notification to update the Teams Apps
        await SendTopic(kudos, serviceBusAdminClient, "kudosdashboard", "notification");



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

    public async Task SendLikeAsync(LikeGateway like, int sign )
    {
        var serviceBusAdminClient = new ServiceBusAdministrationClient(_connectionString);

        //gamification
        await SendTopic($"{like.FromPersonId},{sign}", serviceBusAdminClient, _gamificationLikeSentTopicName, "notification");
        await SendTopic($"{like.ToPersonId},{sign}", serviceBusAdminClient, _gamificationLikeReceivedTopicName, "notification");

        //notification to update the Teams Apps
        await SendTopic(like, serviceBusAdminClient, "dashboard", "notification");
    }

    public async Task MessageSent(CommentsRequest comments)
    {
        //create an admin client to manage artifacts

        var serviceBusAdminClient = new ServiceBusAdministrationClient(_connectionString);

        //gamification
        await SendTopic(comments.FromPersonId, serviceBusAdminClient, _gamificationMessageSentTopicName, "notification");
        await SendTopic(comments.ToPersonId, serviceBusAdminClient, _gamificationMessageReceivedTopicName, "notification");



        //we dont need to notify the Teams App, since the Kudos database will send a notification
    }

    public async Task MessageDeleted(CommentsRequest comments)
    {
        //create an admin client to manage artifacts

        var serviceBusAdminClient = new ServiceBusAdministrationClient(_connectionString);

        //gamification
        await SendTopic(comments.FromPersonId, serviceBusAdminClient, _gamificationMessageDeletedTopicName, "notification");
        await SendTopic(comments.ToPersonId, serviceBusAdminClient, _gamificationMessageDeletedTopicName, "notification");
    }
}
