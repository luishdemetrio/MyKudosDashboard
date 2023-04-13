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

    private static string _gamificationUndolikeSentTopicName = string.Empty;
    private static string _gamificationUndolikeReceivedTopicName = string.Empty;


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

        _gamificationUndolikeSentTopicName = configuration["KudosServiceBus_GamificationUndolikeSentTopicName"];
        _gamificationUndolikeReceivedTopicName = configuration["KudosServiceBus_GamificationUndolikeReceivedTopicName"];

        _gamificationMessageSentTopicName = configuration["KudosServiceBus_GamificationMessageSentTopicName"];
        _gamificationMessageReceivedTopicName = configuration["KudosServiceBus_GamificationMessageReceivedTopicName"];
        
        _gamificationMessageDeletedFromTopicName = configuration["KudosServiceBus_GamificationMessageDeletedFromTopicName"];
        _gamificationMessageDeletedToTopicName = configuration["KudosServiceBus_GamificationMessageDeletedToTopicName"];

        _serviceBusClient = new ServiceBusClient(_connectionString);

        CreateTopicIfNotExistsAsync(_gamificationKudosSentTopicName).ConfigureAwait(false);
        CreateTopicIfNotExistsAsync(_gamificationKudosReceivedTopicName).ConfigureAwait(false); 

        CreateTopicIfNotExistsAsync(_gamificationLikeSentTopicName).ConfigureAwait(false);
        CreateTopicIfNotExistsAsync(_gamificationLikeReceivedTopicName).ConfigureAwait(false);

        CreateTopicIfNotExistsAsync(_gamificationUndolikeSentTopicName).ConfigureAwait(false);
        CreateTopicIfNotExistsAsync(_gamificationUndolikeReceivedTopicName).ConfigureAwait(false);

        CreateTopicIfNotExistsAsync(_gamificationMessageSentTopicName).ConfigureAwait(false);
        CreateTopicIfNotExistsAsync(_gamificationMessageReceivedTopicName).ConfigureAwait(false);

        CreateTopicIfNotExistsAsync(_gamificationMessageDeletedFromTopicName).ConfigureAwait(false);
        CreateTopicIfNotExistsAsync(_gamificationMessageDeletedToTopicName).ConfigureAwait(false);


        CreateTopicIfNotExistsAsync("likedashboard").ConfigureAwait(false);
        CreateTopicIfNotExistsAsync("undolikedashboard").ConfigureAwait(false);
    }

    private async Task CreateTopicIfNotExistsAsync(string topicName)
    {
        var serviceBusAdminClient = new ServiceBusAdministrationClient(_connectionString);

        if (!await serviceBusAdminClient.TopicExistsAsync(topicName))
        {
            await serviceBusAdminClient.CreateTopicAsync(topicName);
        }
    }


    private async Task SendTopic(object queueMessage, string topic, string subject)
    {

        var sender = _serviceBusClient.CreateSender(topic);

        var message = new ServiceBusMessage(JsonConvert.SerializeObject(queueMessage))
        {
            Subject = subject,
            ContentType = "application/json"
        };

        await  sender.SendMessageAsync(message);
        
        await sender.CloseAsync();
    }


    private static async Task SendQueue(object queueMessage,  string queueName)
    {

     
        var client = new ServiceBusClient(_connectionString);

        var sender = client.CreateSender(queueName);

        var message = new ServiceBusMessage(JsonConvert.SerializeObject(queueMessage))
        {
            Subject = queueName,
            ContentType = "application/json"
        };

        await sender.SendMessageAsync(message);

        await sender.CloseAsync();
    }


    public async Task SendKudosAsync(string kudosId, KudosNotification kudos)
    {

        //send notification via Bot
        await SendQueue(kudos, _notificationTopicName);

        //gamification
        await SendTopic(kudos.From.Id, _gamificationKudosSentTopicName, "FromPersonId");
        await SendTopic(kudos.To.Id, _gamificationKudosReceivedTopicName, "ToPersonId");

        //notification to update the Teams Apps
        await SendTopic(
            new KudosResponse
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
            "kudosdashboard", "KudosResponse");
    }



    public async Task SendLikeAsync(LikeGateway like )
    {
        var serviceBusAdminClient = new ServiceBusAdministrationClient(_connectionString);

        //gamification
        await SendTopic(like.FromPerson.Id, _gamificationLikeSentTopicName, "FromPersonId");
        await SendTopic(like.ToPersonId, _gamificationLikeReceivedTopicName, "ToPersonId");

        //notification to update the Teams Apps
        await SendTopic(like, "likedashboard", "LikeGateway");
    }

    public async Task SendDislikeAsync(LikeGateway like)
    {
        var serviceBusAdminClient = new ServiceBusAdministrationClient(_connectionString);

        //gamification
        await SendTopic(like.FromPerson.Id, _gamificationUndolikeSentTopicName, "FromPersonId");
        await SendTopic(like.ToPersonId, _gamificationUndolikeReceivedTopicName, "ToPersonId");

        //notification to update the Teams Apps
        await SendTopic(like, "undolikedashboard", "LikeGateway");
    }

    public async Task MessageSent(CommentsRequest comments)
    {
        //create an admin client to manage artifacts

        var serviceBusAdminClient = new ServiceBusAdministrationClient(_connectionString);

        //gamification
        await SendQueue(comments.FromPersonId, "GamificationReplySent");
        await SendQueue(comments.ToPersonId, "GamificationReplyReceived");



        //we dont need to notify the Teams App, since the Kudos database will send a notification
    }

    public async Task MessageDeleted(CommentsRequest comments)
    {
        //create an admin client to manage artifacts

        var serviceBusAdminClient = new ServiceBusAdministrationClient(_connectionString);

        //gamification
        await SendQueue(comments.FromPersonId, _gamificationMessageDeletedFromTopicName);
        await SendQueue(comments.ToPersonId, _gamificationMessageDeletedToTopicName);
    }
}
