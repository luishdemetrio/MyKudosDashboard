using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Domain.Models;
using MyKudos.MessageSender.Interfaces;
using MyKudos.MessageSender.Services;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Gateway.Queues;

public class CommentsMessageSender : ICommentsMessageSender
{
 

    private static string _messageSentDashboard = string.Empty;
    private static string _messageDeletedDashboard = string.Empty;
    private static string _messageUpdatedDashboard = string.Empty;

    private static string _notifyUserPoints = string.Empty;

    private readonly IUserPointsService _userPointsService;

    private EventHubMessageSender _eventHubScore;
    private EventHubMessageSender _eventHubCommentSent;
    private EventHubMessageSender _eventHubCommentDeleted;
    private EventHubMessageSender _eventHubCommentUpdated;

    public CommentsMessageSender( IConfiguration configuration, IUserPointsService userPointsService)
    {
               
        _userPointsService = userPointsService;

        _eventHubScore = new EventHubMessageSender(configuration["EventHub_ScoreConnectionString"],
                                                  configuration["EventHub_ScoreName"]);


        _eventHubCommentSent = new EventHubMessageSender(configuration["EventHub_CommentSentConnectionString"],
                                                         configuration["EventHub_CommentSentName"]);

        _eventHubCommentDeleted = new EventHubMessageSender(configuration["EventHub_CommentDeletedConnectionString"],
                                                     configuration["EventHub_CommentDeletedName"]);

        _eventHubCommentUpdated = new EventHubMessageSender(configuration["EventHub_CommentUpdatedConnectionString"],
                                                    configuration["EventHub_CommentUpdatedName"]);
    }

 

    public async Task UpdateUserScore(Kudos.Domain.Models.UserPointScore userPointScore)
    {

        //notification to update the Teams Apps
        await _eventHubScore.PublishAsync<Kudos.Domain.Models.UserPointScore>(userPointScore);
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
        await _eventHubCommentSent.PublishAsync<CommentsRequest>(comments);
     
    }

    public async Task MessageDeleted(CommentsRequest comments)
    {
        //notify User Points
        await NotifyUserScore(comments);

        //notification to update the Teams Apps
        await _eventHubCommentDeleted.PublishAsync<CommentsRequest>(comments);
    }

    public async Task MessageUpdated(CommentsRequest comments)
    {
        //notification to update the Teams Apps
        await _eventHubCommentUpdated.PublishAsync<CommentsRequest>(comments);
    }
}
