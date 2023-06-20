using MyKudos.Gateway.Domain.Models;
using System.Collections.Concurrent;

namespace MyKudosDashboard.EventHub;

public class EventHubUserPointsReceived : IEventHubReceived<UserPointScore>
{
    private ConcurrentBag<IObserverEventHub<UserPointScore>> _observers
                        = new ConcurrentBag<IObserverEventHub<UserPointScore>>();

    private EventHubConsumerHelper<UserPointScore> _eventHubScore;

    public EventHubUserPointsReceived(IConfiguration configuration)
    {
        _eventHubScore = new EventHubConsumerHelper<UserPointScore>(
                               configuration["EventHub_ScoreConnectionString"],
                               configuration["EventHub_ScoreName"],
                               configuration["EventHub_blobStorageConnectionString"],
                               configuration["EventHub_blobContainerName"]
                               );

        _eventHubScore.UpdateCallback += (score =>
        {
            if (score != null)
            {
                foreach (IObserverEventHub<UserPointScore> observer in _observers)
                {
                    observer.NotifyUpdate(score);
                }
            }
        });

        _eventHubScore.Start();
    }
    public void Attach(IObserverEventHub<UserPointScore> observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IObserverEventHub<UserPointScore> observer)
    {
       // _observers.TryTake(out observer);
    }


}
