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

    private static string _gamificationDislikeSentTopicName = string.Empty;
    private static string _gamificationDislikeReceivedTopicName = string.Empty;


    private static string _gamificationMessageSentTopicName = string.Empty;
    private static string _gamificationMessageReceivedTopicName = string.Empty;

    private static string _gamificationMessageDeletedFromTopicName = string.Empty;
    private static string _gamificationMessageDeletedToTopicName = string.Empty;


    private ServiceBusClient _serviceBusClient;

    public KudosQueue(IConfiguration configuration)
    {
        _connectionString = configuration["KudosServiceBus_ConnectionString"];
        _notificationTopicName = configuration["KudosServiceBus_TopicName"];
        
        _gamificationKudosSentTopicName = configuration["KudosServiceBus_GamificationKudosSentTopicName"];
        _gamificationKudosReceivedTopicName = configuration["KudosServiceBus_GamificationKudosReceivedTopicName"];

        _gamificationLikeSentTopicName = configuration["KudosServiceBus_GamificationLikeSentTopicName"];
        _gamificationLikeReceivedTopicName = configuration["KudosServiceBus_GamificationLikeReceivedTopicName"];

        _gamificationDislikeSentTopicName = configuration["KudosServiceBus_GamificationDislikeSentTopicName"];
        _gamificationDislikeReceivedTopicName = configuration["KudosServiceBus_GamificationDislikeReceivedTopicName"];

        _gamificationMessageSentTopicName = configuration["KudosServiceBus_GamificationMessageSentTopicName"];
        _gamificationMessageReceivedTopicName = configuration["KudosServiceBus_GamificationMessageReceivedTopicName"];
        
        _gamificationMessageDeletedFromTopicName = configuration["KudosServiceBus_GamificationMessageDeletedFromTopicName"];
        _gamificationMessageDeletedToTopicName = configuration["KudosServiceBus_GamificationMessageDeletedToTopicName"];

        _serviceBusClient = new ServiceBusClient(_connectionString);
    }

    public async Task SendKudosAsync(string kudosId, KudosNotification kudos)
    {

        //create an admin client to manage artifacts

        var serviceBusAdminClient = new ServiceBusAdministrationClient(_connectionString);

        //send notification via Bot
        await SendQueue(kudos, serviceBusAdminClient, _notificationTopicName);

        //gamification
        await SendTopic(kudos.From.Id, serviceBusAdminClient, _gamificationKudosSentTopicName, "FromPersonId");
        await SendTopic(kudos.To.Id, serviceBusAdminClient, _gamificationKudosReceivedTopicName,  "ToPersonId") ;

        //notification to update the Teams Apps
        await SendTopic(new KudosResponse
                        {
                            Id = kudosId,
                            To = kudos.To,
                            From = kudos.From,
                            Message = kudos.Message,
                            Title = kudos.Title.Description,
                            SendOn = kudos.SendOn,
                            Comments = new(),
                            Likes = new List<Person>()
                        }, 
                        serviceBusAdminClient, "kudosdashboard", "KudosResponse");



    }

    private async Task SendTopic(object queueMessage, ServiceBusAdministrationClient serviceBusAdminClient,
                                        string topic, string subject)
    {

        //create a topic if it doesnt exist

        //if (!await serviceBusAdminClient.TopicExistsAsync(topic))
        //{
        //    await serviceBusAdminClient.CreateTopicAsync(topic);
        //}

  
       // var client = new ServiceBusClient(_connectionString);

        var sender = _serviceBusClient.CreateSender(topic);

        var message = new ServiceBusMessage(JsonConvert.SerializeObject(queueMessage))
        {
            Subject = subject,
            ContentType = "application/json"
        };

        await sender.SendMessageAsync(message);

        await sender.CloseAsync();
    }


    private static async Task SendQueue(object queueMessage, ServiceBusAdministrationClient serviceBusAdminClient,
                                        string queueName)
    {

        //create a topic if it doesnt exist

        if (!await serviceBusAdminClient.QueueExistsAsync(queueName))
        {
            await serviceBusAdminClient.CreateQueueAsync(queueName);
        }

        var client = new ServiceBusClient(_connectionString);

        var sender = client.CreateSender(queueName);

        var message = new ServiceBusMessage(JsonConvert.SerializeObject(queueMessage));

        await sender.SendMessageAsync(message);

        await sender.CloseAsync();
    }

    public async Task SendLikeAsync(LikeGateway like )
    {
        var serviceBusAdminClient = new ServiceBusAdministrationClient(_connectionString);

        //gamification
        await SendTopic(like.FromPerson.Id, serviceBusAdminClient, _gamificationLikeSentTopicName, "FromPersonId");
        await SendTopic(like.ToPersonId, serviceBusAdminClient, _gamificationLikeReceivedTopicName, "ToPersonId");

        //notification to update the Teams Apps
        await SendTopic(like, serviceBusAdminClient, "likedashboard", "LikeGateway");
    }

    public async Task SendDislikeAsync(LikeGateway like)
    {
        var serviceBusAdminClient = new ServiceBusAdministrationClient(_connectionString);

        //gamification
        await SendTopic(like.FromPerson.Id, serviceBusAdminClient, _gamificationDislikeSentTopicName, "FromPersonId");
        await SendTopic(like.ToPersonId, serviceBusAdminClient, _gamificationDislikeReceivedTopicName, "ToPersonId");

        //notification to update the Teams Apps
        await SendTopic(like, serviceBusAdminClient, "likedashboard", "LikeGateway");
    }

    public async Task MessageSent(CommentsRequest comments)
    {
        //create an admin client to manage artifacts

        var serviceBusAdminClient = new ServiceBusAdministrationClient(_connectionString);

        //gamification
        await SendQueue(comments.FromPersonId, serviceBusAdminClient, "GamificationReplySent");
        await SendQueue(comments.ToPersonId, serviceBusAdminClient, "GamificationReplyReceived");



        //we dont need to notify the Teams App, since the Kudos database will send a notification
    }

    public async Task MessageDeleted(CommentsRequest comments)
    {
        //create an admin client to manage artifacts

        var serviceBusAdminClient = new ServiceBusAdministrationClient(_connectionString);

        //gamification
        await SendQueue(comments.FromPersonId, serviceBusAdminClient, _gamificationMessageDeletedFromTopicName);
        await SendQueue(comments.ToPersonId, serviceBusAdminClient, _gamificationMessageDeletedToTopicName);
    }
}
