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
    private EventHubMessageSender _eventHubKudosDeleted;
    private EventHubMessageSender _eventHubKudosUpdated;

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

        _eventHubKudosDeleted = new EventHubMessageSender(configuration["EventHub_KudosDeletedConnectionString"],
                                                  configuration["EventHub_KudosDeletedName"]);

        _eventHubKudosUpdated = new EventHubMessageSender(configuration["EventHub_KudosUpdatedConnectionString"],
                                                 configuration["EventHub_KudosUpdatedName"]);
    }

 


    public async Task SendKudosAsync(int kudosId, Gateway.Domain.Models.KudosNotification kudos)
    {

        //notification to update the Teams Apps

        await _eventHubKudosSent.PublishAsync<KudosResponse>(
            new KudosResponse
            {
                Id = kudosId,
                Receivers = kudos.Receivers,
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

        foreach (var receiver in kudos.Receivers)
        {
            var userPointsReceiver = await _userPointsService.GetUserScoreAsync(receiver.Id);
            await UpdateUserScore(userPointsReceiver);
        }

        
    }


    public async Task UpdateUserScore(Kudos.Domain.Models.UserPointScore userPointScore)
    {
        //notification to update the Teams Apps
        await _eventHubScore.PublishAsync<Kudos.Domain.Models.UserPointScore>(userPointScore);
       
    }


    public async Task SendLikeAsync(LikeGateway like, List<KudosReceiver> recognized)
    {
        await ProcessLikeAsync(like, recognized, _eventHubLikeSent.PublishAsync);
    }

    public async Task SendUndoLikeAsync(LikeGateway like, List<KudosReceiver> recognized)
    {
        await ProcessLikeAsync(like, recognized, _eventHubUndoLikeSent.PublishAsync);
    }

    private async Task ProcessLikeAsync(LikeGateway like, List<KudosReceiver> recognized, Func<LikeGateway, Task> publishAsync)
    {
        //notification to update the Teams Apps
        await publishAsync(like);

        //get the user points of who sent to update the Teams Dashboard
        var userPointsSender = await _userPointsService.GetUserScoreAsync(like.FromPerson.Id);
        await UpdateUserScore(userPointsSender);

        //need to update the points of who won recognition
        foreach (var winner in recognized)
        {
            //the equality can happens when the person who received the kudos comments on his/her kudos to thanks
            //in this case we dont need to notify it again
            if (like.FromPerson.Id != winner.ToPersonId)
            {
                //get the user points of who received to update the Teams Dashboard
                var userPointsReceiver = await _userPointsService.GetUserScoreAsync(winner.ToPersonId);
                await UpdateUserScore(userPointsReceiver);
            }
        }
    }

    public async Task KudosDeleted(int kudosId, Kudos.Domain.Models.Kudos kudos)
    {

        //notification to update the Teams Apps

        await _eventHubKudosDeleted.PublishAsync<int>(kudosId);

        //get the user points of who sent to update the Teams Dashboard
        var userPointsSender = await _userPointsService.GetUserScoreAsync(kudos.UserFrom.UserProfileId);
        await UpdateUserScore(userPointsSender);

        //get the user points of who received to update the Teams Dashboard

        foreach (var receiver in kudos.Recognized)
        {
            var userPointsReceiver = await _userPointsService.GetUserScoreAsync(receiver.ToPersonId);
            await UpdateUserScore(userPointsReceiver);
        }


    }

    public async Task KudosUpdated(Domain.Models.KudosMessage kudos)
    {
        //notification to update the Teams Apps

        await _eventHubKudosUpdated.PublishAsync<Domain.Models.KudosMessage>(kudos);
    }
}
