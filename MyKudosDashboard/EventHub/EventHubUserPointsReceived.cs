using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.EventGrid;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace MyKudosDashboard.EventHub;

public class EventHubUserPointsReceived : IEventHubReceived<UserPointScore>
{
    private ConcurrentQueue<IObserverEventHub<UserPointScore>> _observers
                        = new ConcurrentQueue<IObserverEventHub<UserPointScore>>();

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
        _observers.Enqueue(observer);
    }

    public void Detach(IObserverEventHub<UserPointScore> observer)
    {
        IObserverEventHub<UserPointScore> removedObserver;

        while (!_observers.IsEmpty)
        {
            _observers.TryDequeue(out removedObserver);
            if (removedObserver != observer)
                _observers.Enqueue(removedObserver);
        }
    }

   
}
