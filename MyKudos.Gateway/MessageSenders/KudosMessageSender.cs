using Azure.Messaging.ServiceBus.Administration;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Domain.Models;
using MyKudos.MessageSender.Services;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Gateway.Queues;

public class KudosMessageSender : IKudosMessageSender
{
        
    private static string _connectionString = string.Empty;
    
    private static string _likeSentDashboard = string.Empty;
    private static string _likeUndoDashboard = string.Empty;

    private static string _calculateUserScoreTopicEndPoint = string.Empty;
    private static string _calculateUserStoreTopicKey = string.Empty;

    private static string _dashboardTopicEndPoint = string.Empty;
    private static string _dashboardTopicKey = string.Empty;

    private EventGridMessageSender _calculateScore;

    private EventGridMessageSender _dashboardTopic;


    private readonly IUserPointsService _userPointsService;


    public KudosMessageSender(IConfiguration configuration,
                              IUserPointsService userPointsService)
    {       

        ReadConfigurationSettings(configuration);

        _userPointsService = userPointsService;

        _calculateScore = new EventGridMessageSender(_calculateUserScoreTopicEndPoint, _calculateUserStoreTopicKey);
        _dashboardTopic = new EventGridMessageSender(_dashboardTopicEndPoint, _dashboardTopicKey);
        

    }

    private static void ReadConfigurationSettings(IConfiguration configuration)
    {
        _calculateUserScoreTopicEndPoint = configuration["EventGrid_CalculateUserScoreTopic_Endpoint"];
        _calculateUserStoreTopicKey = configuration["EventGrid_CalculateUserScoreTopic_Key"];

        _dashboardTopicEndPoint = configuration["EventGrid_DashboardTopic_Endpoint"];
        _dashboardTopicKey = configuration["EventGrid_DashboardTopic_Key"];

        _connectionString = configuration["KudosServiceBus_ConnectionString"];

        _likeSentDashboard = configuration["KudosServiceBus_LikeSentDashboard"];
        _likeUndoDashboard = configuration["KudosServiceBus_LikeUndoDashboard"];

    }


    public async Task SendKudosAsync(int kudosId, Gateway.Domain.Models.KudosNotification kudos)
    {
        //send notification to generate the adaptive card
        //await _messageSender.SendQueue(kudos, _notificationTopicName);

        await _calculateScore.SendTopic(kudos.From.Id, "SendKudosFrom", "CalculateUserScore");
        await _calculateScore.SendTopic(kudos.To.Id, "SendKudosTo", "CalculateUserScore");

        //notification to update the Teams Apps
        await _dashboardTopic.SendTopic(
            new KudosResponse
            {
                Id = kudosId,
                To = kudos.To,
                From = kudos.From,
                Message = kudos.Message,
                Title = kudos.Reward.Title,
                SendOn = kudos.SendOn,
                Comments = new List<int>(),
                Likes = new List<Gateway.Domain.Models.Person>()
            },
            "SendKudos", "SendKudosDashboard");

        //get the user points of who sent to update the Teams Dashboard
        var userPointsSender = await _userPointsService.GetUserScoreAsync(kudos.From.Id);
        await UpdateUserScore(userPointsSender);

        //get the user points of who received to update the Teams Dashboard
        var userPointsReceiver = await _userPointsService.GetUserScoreAsync(kudos.To.Id);
        await UpdateUserScore(userPointsReceiver);
    }


    public async Task UpdateUserScore(Kudos.Domain.Models.UserPointScore userPointScore)
    {
        
        //notification to update the Teams Apps
        await _dashboardTopic.SendTopic(userPointScore,  "UpdateScore", "UpdateUserPointDashboard");
    }


    public async Task SendLikeAsync(LikeGateway like)
    {
        var serviceBusAdminClient = new ServiceBusAdministrationClient(_connectionString);        

        //notification to update the Teams Apps
        await _dashboardTopic.SendTopic(like, _likeSentDashboard, _likeSentDashboard);


        //get the user points of who sent to update the Teams Dashboard
        var userPointsSender = await _userPointsService.GetUserScoreAsync(like.FromPerson.Id);
        await UpdateUserScore(userPointsSender);

        if (like.FromPerson.Id != like.ToPersonId)
        {
            //the equality can happens when the person who received the kudos comments on his/her kudos to thanks
            //in this case we dont need to notify it again

            //get the user points of who received to update the Teams Dashboard
            var userPointsReceiver = await _userPointsService.GetUserScoreAsync(like.ToPersonId);
            await UpdateUserScore(userPointsReceiver);
        }
    }

    public async Task SendUndoLikeAsync(LikeGateway like)
    {
        var serviceBusAdminClient = new ServiceBusAdministrationClient(_connectionString);

        //notification to update the Teams Apps
        await _dashboardTopic.SendTopic(like, _likeUndoDashboard, _likeUndoDashboard);

        //get the user points of who sent to update the Teams Dashboard
        var userPointsSender = await _userPointsService.GetUserScoreAsync(like.FromPerson.Id);
        await UpdateUserScore(userPointsSender);

        if (like.FromPerson.Id != like.ToPersonId)
        {
            //the equality can happens when the person who received the kudos comments on his/her kudos to thanks
            //in this case we dont need to notify it again
            //get the user points of who received to update the Teams Dashboard
            var userPointsReceiver = await _userPointsService.GetUserScoreAsync(like.ToPersonId);
            await UpdateUserScore(userPointsReceiver);
        }
    }


}
