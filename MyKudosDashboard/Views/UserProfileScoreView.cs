using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.EventHub;
using MyKudosDashboard.Interfaces;

namespace MyKudosDashboard.Views;

public class UserProfileScoreView : IUserProfileScoreView, IObserverEventHub<UserPointScore>
{

    private IGamificationGateway _gamificationGateway;

    public IUserProfileScoreView.UpdateScoreCallBack UserScoreCallback { get; set; }

    private ILogger<UserProfileScoreView> _logger;

    private IEventHubReceived<UserPointScore> _eventHubUserPointsReceived;

    
    public UserProfileScoreView(IGamificationGateway gamificationGateway, 
                                ILogger<UserProfileScoreView> logger,
                                IEventHubReceived<UserPointScore> eventHubUserPointsReceived
                                )
    {
        _logger = logger;

        _gamificationGateway = gamificationGateway;

       
        _eventHubUserPointsReceived = eventHubUserPointsReceived;

        RegisterObserver();

    }

    public void RegisterObserver()
    {
        _eventHubUserPointsReceived.Attach(this);
    }

    //public void UnregisterObserver(string userId)
    //{
    //    _eventHubUserPointsReceived.Detach(userId);
    //}

    public async Task<UserPointScore> GetUserScore(string userId)
    {
        return await _gamificationGateway.GetUserScoreAsync(userId);
    }

   
    public void NotifyUpdate(UserPointScore score)
    {
        _logger.LogInformation($"Score received: \nScore: {score.Score}\nKudos sent: {score.KudosSent}\nLikes Received: {score.LikesReceived}");

        UserScoreCallback?.Invoke(score);
    }

    //public void Dispose()
    //{
    //    UnregisterObserver(_userId);
    //}
}
