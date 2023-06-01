using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Domain.Models;
using MyKudos.MessageSender.Interfaces;
using MyKudos.MessageSender.Services;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Gateway.Queues;

public class CommentsMessageSender : ICommentsMessageSender
{
    private static string _calculateUserScoreTopicEndPoint = string.Empty;
    private static string _calculateUserStoreTopicKey = string.Empty;

    private static string _dashboardTopicEndPoint = string.Empty;
    private static string _dashboardTopicKey = string.Empty;

    private static string _messageSentDashboard = string.Empty;
    private static string _messageDeletedDashboard = string.Empty;
    private static string _messageUpdatedDashboard = string.Empty;

    private static string _notifyUserPoints = string.Empty;

    private readonly IUserPointsService _userPointsService;

    private EventGridMessageSender _calculateScore;
    private EventGridMessageSender _dashboardTopic;


    public CommentsMessageSender( IConfiguration configuration, IUserPointsService userPointsService)
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

        _messageSentDashboard = configuration["EventGrid_MessageSentDashboard"];
        _messageDeletedDashboard = configuration["EventGrid_MessageDeletedDashboard"];
        _messageUpdatedDashboard = configuration["EventGrid_MessageUpdatedDashboard"];

        _notifyUserPoints = configuration["EventGrid_NotifyUserPoints"];

    }

    public async Task UpdateUserScore(Kudos.Domain.Models.UserPointScore userPointScore)
    {

        //notification to update the Teams Apps
        await _dashboardTopic.SendTopic(userPointScore, "UpdateScore", "UpdateUserPointDashboard");
    }

    private async Task NotifyUserScore(CommentsRequest comments)
    {
        //get the user points of who sent to update the Teams Dashboard
        var userPointsSender = await _userPointsService.GetUserScoreAsync(comments.FromPersonId);
        await UpdateUserScore(userPointsSender);

        if (comments.ToPersonId != comments.FromPersonId)
        {
            //the equality can happens when the person who received the kudos comments on his/her kudos to thanks
            //in this case we dont need to notify it again

            //get the user points of who received to update the Teams Dashboard
            var userPointsReceiver = await _userPointsService.GetUserScoreAsync(comments.ToPersonId);
            await UpdateUserScore(userPointsReceiver);            
        }

    }
    public async Task MessageSent(CommentsRequest comments)
    {
        //notify User Points
        await NotifyUserScore(comments);

        //notification to update the Teams Apps
        await _dashboardTopic.SendTopic(comments, _messageSentDashboard, "MessageSent");
    }

    public async Task MessageDeleted(CommentsRequest comments)
    {
        //notify User Points
        await NotifyUserScore(comments);

        //notification to update the Teams Apps
        await _dashboardTopic.SendTopic(comments, _messageDeletedDashboard, "MessageDeleted");
    }

    public async Task MessageUpdated(CommentsRequest comments)
    {
        //notification to update the Teams Apps
        await _dashboardTopic.SendTopic(comments, _messageUpdatedDashboard, "MessageUpdated");
    }
}
