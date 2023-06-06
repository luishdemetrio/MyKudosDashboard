using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.EventHub;
using MyKudosDashboard.Interfaces;

namespace MyKudosDashboard.Views;

public class UserProfileScoreView : IUserProfileScoreView, IObserverEventHub<UserPointScore>
{

    private IGamificationGateway _gamificationGateway;

    public IUserProfileScoreView.UpdateScoreCallBack UserScoreCallback { get; set; }

    private ILogger<UserProfileScoreView> _logger;


    public UserProfileScoreView(IGamificationGateway gamificationGateway, 
                                ILogger<UserProfileScoreView> logger,
                                IEventHubReceived<UserPointScore> eventHubUserPointsReceived
                                )
    {
        _logger = logger;

        _gamificationGateway = gamificationGateway;

        eventHubUserPointsReceived.Attach(this);
        


    }

    public async Task<UserPointScore> GetUserScore(string userId)
    {
        return await _gamificationGateway.GetUserScoreAsync(userId);
    }

   
    public void NotifyUpdate(UserPointScore score)
    {
        _logger.LogInformation($"Score received: \nScore: {score.Score}\nKudos sent: {score.KudosSent}\nLikes Received: {score.LikesReceived}");

        UserScoreCallback?.Invoke(score);
    }

    
}
