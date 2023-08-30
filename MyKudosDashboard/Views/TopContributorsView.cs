using SuperKudos.Aggregator.Domain.Models;
using MyKudosDashboard.EventHub;
using MyKudosDashboard.Interfaces;

namespace MyKudosDashboard.Views;

public class TopContributorsView : ITopContributorsView, IObserverEventHub<UserPointScore>, IDisposable
{

    private IGamificationGateway _gamificationGateway;

    public ITopContributorsView.UpdateTopContributorsCallBack TopContributorsCallBack { get ; set ; }

    private IEventHubUserPointsReceived _eventHubUserPointsReceived;

    private ILogger<TopContributorsView> _logger;

    private string _userId;

    public TopContributorsView(IGamificationGateway gamificationGateway,
                              ILogger<TopContributorsView> logger,
                               IEventHubUserPointsReceived eventHubUserPointsReceived)
    {
        _gamificationGateway = gamificationGateway;

        _eventHubUserPointsReceived = eventHubUserPointsReceived;

        _logger = logger;
    }

    public void RegisterObserver(string userId)
    {
        _userId = userId;
        _eventHubUserPointsReceived.Attach($"top_{userId}", this);
    }

    public void UnregisterObserver(string userId)
    {
        _eventHubUserPointsReceived.Detach($"top_{userId}");
    }


    public async Task<IEnumerable<TopContributors>> GetTopContributors()
    {
        return await _gamificationGateway.GetTopContributors();
    }

    public void NotifyUpdate(UserPointScore score)
    {
        _logger.LogInformation($"Top Contributors received: \n {System.Text.Json.JsonSerializer.Serialize<UserPointScore>(score)}");

        TopContributorsCallBack?.Invoke();
    }

    public void Dispose()
    {
        UnregisterObserver(_userId);
    }
}
