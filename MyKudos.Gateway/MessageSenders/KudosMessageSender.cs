using Azure.Messaging.ServiceBus.Administration;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Domain.Models;
using MyKudos.MessageSender.Interfaces;
using MyKudos.MessageSender.Services;

namespace MyKudos.Gateway.Queues;

public class KudosMessageSender : IKudosMessageSender
{

    
    private static string _connectionString = string.Empty;
    private static string _notificationTopicName = string.Empty;

    private static string _kudosSentDashboard = string.Empty;

    private static string _likeSentDashboard = string.Empty;
    private static string _likeUndoDashboard = string.Empty;

    
   // private static string _notifyUserPoints = string.Empty;

    private static string _calculateUserScoreTopicEndPoint = string.Empty;
    private static string _calculateUserStoreTopicKey = string.Empty;

    private EventGridMessageSender _calculateScore; 

    public KudosMessageSender( IConfiguration configuration)
    {       

        ReadConfigurationSettings(configuration);        

        _calculateScore = new EventGridMessageSender(_calculateUserScoreTopicEndPoint, _calculateUserStoreTopicKey);


        //_messageSender.CreateTopicIfNotExistsAsync(_kudosSentDashboard).ConfigureAwait(false);
        //_messageSender.CreateTopicIfNotExistsAsync(_likeSentDashboard).ConfigureAwait(false);
        //_messageSender.CreateTopicIfNotExistsAsync(_likeUndoDashboard).ConfigureAwait(false);

       // _messageSender.CreateQueueIfNotExistsAsync(_notifyUserPoints).ConfigureAwait(false);
        
    }

    private static void ReadConfigurationSettings(IConfiguration configuration)
    {

        _calculateUserScoreTopicEndPoint = configuration["EventGrid_CalculateUserScoreTopic_Endpoint"];
        _calculateUserStoreTopicKey = configuration["EventGrid_CalculateUserScoreTopic_Key"];

        _connectionString = configuration["KudosServiceBus_ConnectionString"];

        _notificationTopicName = configuration["KudosServiceBus_TopicName"];

        //_notifyUserPoints = configuration["KudosServiceBus_NotifyUserPoints"];       

        _likeSentDashboard = configuration["KudosServiceBus_LikeSentDashboard"];
        _likeUndoDashboard = configuration["KudosServiceBus_LikeUndoDashboard"];

        _kudosSentDashboard = configuration["KudosServiceBus_KudosSentDashboard"];

        _notificationTopicName = configuration["KudosServiceBus_AgentTopicName"];
    }


    public async Task SendKudosAsync(int kudosId, Gateway.Domain.Models.KudosNotification kudos)
    {

        //send notification to generate the adaptive card
        //await _messageSender.SendQueue(kudos, _notificationTopicName);

        //notify User Points
        //await _messageSender.SendQueue(kudos.From.Id, _notifyUserPoints);
        //await _messageSender.SendQueue(kudos.To.Id, _notifyUserPoints);

        await _calculateScore.SendTopic<string>(kudos.From.Id, "SendKudosFrom", "CalculateUserScore");
        await _calculateScore.SendTopic<string>(kudos.To.Id, "SendKudosTo", "CalculateUserScore");

        //notification to update the Teams Apps
        //await _messageSender.SendTopic(
        //    new KudosResponse
        //    {
        //        Id = kudosId,
        //        To = kudos.To,
        //        From = kudos.From,
        //        Message = kudos.Message,
        //        Title = kudos.Reward.Title,
        //        SendOn = kudos.SendOn,
        //        Comments = new List<int>(),
        //        Likes = new List<Gateway.Domain.Models.Person>()
        //    },
        //    _kudosSentDashboard, _kudosSentDashboard);
    }


    public async Task SendLikeAsync(LikeGateway like)
    {
        var serviceBusAdminClient = new ServiceBusAdministrationClient(_connectionString);        

        //notify User Points
        await _calculateScore.SendTopic<string>(like.FromPerson.Id, "SendLikesFrom", "CalculateUserScore");
        await _calculateScore.SendTopic<string>(like.ToPersonId,  "SendLikesTo", "CalculateUserScore");


        //notification to update the Teams Apps
       // await _messageSender.SendTopic(like, _likeSentDashboard, _likeSentDashboard);
    }

    public async Task SendUndoLikeAsync(LikeGateway like)
    {
        var serviceBusAdminClient = new ServiceBusAdministrationClient(_connectionString);
      
        //notify User Points
        await _calculateScore.SendTopic<string>(like.FromPerson.Id, "UndoLikeFrom", "CalculateUserScore");
        await _calculateScore.SendTopic<string>(like.ToPersonId,  "UndoLikeTo", "CalculateUserScore");

        //notification to update the Teams Apps
        //await _messageSender.SendTopic(like, _likeUndoDashboard, _likeUndoDashboard);
    }


}
