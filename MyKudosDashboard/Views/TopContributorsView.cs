using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.EventHub;
using MyKudosDashboard.Interfaces;

namespace MyKudosDashboard.Views;

public class TopContributorsView : ITopContributorsView, IObserverEventHub<UserPointScore>//, IDisposable
{

    private IGamificationGateway _gamificationGateway;

    public ITopContributorsView.UpdateTopContributorsCallBack TopContributorsCallBack { get ; set ; }

    private IEventHubReceived<UserPointScore> _eventHubUserPointsReceived;

    
    public TopContributorsView(IGamificationGateway gamificationGateway,
                               IEventHubReceived<UserPointScore> eventHubUserPointsReceived)
    {
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


    public async Task<IEnumerable<TopContributors>> GetTopContributors()
    {
        return await _gamificationGateway.GetTopContributors();
    }

    public void NotifyUpdate(UserPointScore score)
    {
        TopContributorsCallBack?.Invoke();
    }

    //public void Dispose()
    //{
    // //   UnregisterObserver(_userId);
    //}
}
