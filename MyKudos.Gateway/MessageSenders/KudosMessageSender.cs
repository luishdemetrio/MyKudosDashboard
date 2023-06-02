using Azure.Messaging.ServiceBus.Administration;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Domain.Models;
using MyKudos.MessageSender.Services;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Gateway.Queues;

public class KudosMessageSender : IKudosMessageSender
{
        
    
    private EventHubMessageSender _eventHubScore;
    private EventHubMessageSender _eventHubLikeSent;
    private EventHubMessageSender _eventHubUndoLikeSent;
    private EventHubMessageSender _eventHubKudosSent;

    private readonly IUserPointsService _userPointsService;


    
    public KudosMessageSender(IConfiguration configuration,
                              IUserPointsService userPointsService)
    {       


        _userPointsService = userPointsService;


        _eventHubScore = new EventHubMessageSender(configuration["EventHub_ScoreConnectionString"],
                                                   configuration["EventHub_ScoreName"]);


        _eventHubLikeSent = new EventHubMessageSender(configuration["EventHub_LikeSentConnectionString"],
                                                      configuration["EventHub_LikeSentName"]);

        _eventHubUndoLikeSent = new EventHubMessageSender(configuration["EventHub_UndoLikeSentConnectionString"],
                                                     configuration["EventHub_UndoLikeSentName"]);

        _eventHubKudosSent = new EventHubMessageSender(configuration["EventHub_KudosSentConnectionString"],
                                                    configuration["EventHub_KudosSentName"]);
    }

 


    public async Task SendKudosAsync(int kudosId, Gateway.Domain.Models.KudosNotification kudos)
    {

        //notification to update the Teams Apps

        await _eventHubLikeSent.PublishAsync<KudosResponse>(
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
            });

        //get the user points of who sent to update the Teams Dashboard
        var userPointsSender = await _userPointsService.GetUserScoreAsync(kudos.From.Id);
        await UpdateUserScore(userPointsSender);

        //get the user points of who received to update the Teams Dashboard
        var userPointsReceiver = await _userPointsService.GetUserScoreAsync(kudos.To.Id);
        await UpdateUserScore(userPointsReceiver);

        
    }


    public async Task UpdateUserScore(Kudos.Domain.Models.UserPointScore userPointScore)
    {
        await _eventHubScore.PublishAsync<Kudos.Domain.Models.UserPointScore>(userPointScore);
        //notification to update the Teams Apps
       // await _dashboardTopic.SendTopic(userPointScore,  "UpdateScore", "UpdateUserPointDashboard");
    }


    public async Task SendLikeAsync(LikeGateway like)
    {
      //notification to update the Teams Apps
        await _eventHubLikeSent.PublishAsync<LikeGateway>(like);

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

        //notification to update the Teams Apps
        await _eventHubUndoLikeSent.PublishAsync<LikeGateway>(like);

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
