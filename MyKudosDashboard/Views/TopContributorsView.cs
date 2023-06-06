using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.EventHub;
using MyKudosDashboard.Interfaces;

namespace MyKudosDashboard.Views;

public class TopContributorsView : ITopContributorsView, IObserverEventHub<UserPointScore>
{

    private IGamificationGateway _gamificationGateway;

    public ITopContributorsView.UpdateTopContributorsCallBack TopContributorsCallBack { get ; set ; }


    public TopContributorsView(IGamificationGateway gamificationGateway,
                               IEventHubReceived<UserPointScore> eventHubUserPointsReceived)
    {
        _gamificationGateway = gamificationGateway;

        eventHubUserPointsReceived.Attach(this);
    }

    
    public async Task<IEnumerable<TopContributors>> GetTopContributors()
    {
        return await _gamificationGateway.GetTopContributors();
    }

    public void NotifyUpdate(UserPointScore score)
    {
        TopContributorsCallBack?.Invoke();
    }
}
