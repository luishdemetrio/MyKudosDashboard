using SuperKudos.Aggregator.Domain.Models;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace MyKudosDashboard.EventHub;

public class EventHubUserPointsReceived : IEventHubUserPointsReceived
{
    private ConcurrentDictionary<string, IObserverEventHub<UserPointScore>> _observers
                        = new ();

    private EventHubConsumerHelper<UserPointScore> _eventHub;

    private ILogger<EventHubUserPointsReceived> _logger;

    public EventHubUserPointsReceived(IConfiguration configuration, ILogger<EventHubUserPointsReceived> logger)
    {

        _logger = logger;

        _eventHub = new EventHubConsumerHelper<UserPointScore>(
                               configuration["EventHub_ScoreConnectionString"],
                               configuration["EventHub_ScoreName"],
                               configuration["EventHub_blobStorageConnectionString"],
                               configuration["EventHub_blobContainerName"]
                               );

        _eventHub.UpdateCallback += (score => 
            {
                if (score != null)
                {
                    foreach (IObserverEventHub<UserPointScore> observer in _observers.Values)
                    {
                        observer.NotifyUpdate(score);
                    }
                }
            });

        _eventHub.Start().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the exception here
                _logger.LogError(task.Exception.ToString());
                throw task.Exception;
            }
        });
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
