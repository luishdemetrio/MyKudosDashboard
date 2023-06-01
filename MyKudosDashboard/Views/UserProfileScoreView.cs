using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.EventGrid;
using MyKudosDashboard.Interfaces;

namespace MyKudosDashboard.Views;

public class UserProfileScoreView : IUserProfileScoreView, IObserverUserPoints, IDisposable
{

    private IGamificationGateway _gamificationGateway;

    public IUserProfileScoreView.UpdateScoreCallBack UserScoreCallback { get; set; }

    private IEventGridUserPointsReceived _eventGridReceived;

    public UserProfileScoreView(IGamificationGateway gamificationGateway, IConfiguration configuration, ILogger<UserProfileScoreView> logger,
                                IEventGridUserPointsReceived eventGridReceived)
    {
        _gamificationGateway = gamificationGateway;
        _eventGridReceived = eventGridReceived;

        _eventGridReceived.Attach(this);
    }



    public async Task<UserPointScore> GetUserScore(string userId)
    {
        return await _gamificationGateway.GetUserScoreAsync(userId);
    }

    public void Dispose()
    {
        _eventGridReceived.Detach(this);
    }

    public void UpdateUserScore(UserPointScore score)
    {
        UserScoreCallback?.Invoke(score);
    }
}
