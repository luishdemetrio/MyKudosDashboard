using MyKudos.Gateway.Domain.Models;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace MyKudosDashboard.EventHub;

public class EventHubUserPointsReceived : IEventHubReceived<UserPointScore>
{
    private ConcurrentDictionary<string, IObserverEventHub<UserPointScore>> _observers
                        = new ();

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
                    foreach (IObserverEventHub<UserPointScore> observer in _observers.Values)
                    {
                        observer.NotifyUpdate(score);
                    }
                }
            });

        _eventHubScore.Start();
    }
    public void Attach(string userId, IObserverEventHub<UserPointScore> observer)
    {
        _observers.AddOrUpdate(userId, observer,
                    (_, existingObserver) => existingObserver);
    }

    public void Detach(string userId)
    {

        _observers.TryRemove(userId, out _);
    }

}
