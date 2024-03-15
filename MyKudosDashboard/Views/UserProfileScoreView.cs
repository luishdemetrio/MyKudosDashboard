using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.EventHub;
using MyKudosDashboard.Interfaces;

namespace MyKudosDashboard.Views;

public class UserProfileScoreView : IUserProfileScoreView, IObserverEventHub<UserPointScore>, IDisposable
{

    private IGamificationGateway _gamificationGateway;

    public IUserProfileScoreView.UpdateScoreCallBack UserScoreCallback { get; set; }

    private ILogger<UserProfileScoreView> _logger;

    private IEventHubUserPointsReceived _eventHubUserPointsReceived;

    private string _userId;

    private int _currentYear;

    public UserProfileScoreView(IGamificationGateway gamificationGateway, 
                                ILogger<UserProfileScoreView> logger,
                                IEventHubUserPointsReceived eventHubUserPointsReceived,
                                IConfiguration config
                                )
    {
        _logger = logger;

        _gamificationGateway = gamificationGateway;

        _eventHubUserPointsReceived = eventHubUserPointsReceived;

        _currentYear = int.Parse(config["CurrentYear"]);
    }

    public void RegisterObserver(string userId)
    {
        _userId = userId;
        _eventHubUserPointsReceived.Attach(userId, this);
    }

    public void UnregisterObserver(string userId)
    {
        if (userId != null ) 
        _eventHubUserPointsReceived.Detach(userId);
    }

    public async Task<UserPointScore> GetUserScore(string userId, bool justMyTeam )
    {
        return await _gamificationGateway.GetUserScoreAsync(userId, _currentYear, justMyTeam);
    }

   
    public void NotifyUpdate(UserPointScore score)
    {
        _logger.LogInformation($"Userscore received: \n {System.Text.Json.JsonSerializer.Serialize<UserPointScore>(score)}");

        UserScoreCallback?.Invoke(score);
    }

    public void Dispose()
    {
        UnregisterObserver(_userId);
    }
}
