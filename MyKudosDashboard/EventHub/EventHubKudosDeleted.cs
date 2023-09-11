using MyKudos.Gateway.Domain.Models;
using System.Collections.Concurrent;

namespace MyKudosDashboard.EventHub;

public class EventHubKudosDeleted : IEventHubKudosDeleted
{
    private ConcurrentDictionary<string, IObserverEventHub<int>> _observers
                         = new();

    private EventHubConsumerHelper<int> _eventHubKudos;

    private ILogger<EventHubKudosDeleted> _logger;

    public EventHubKudosDeleted(IConfiguration configuration,
                             ILogger<EventHubKudosDeleted> logger)
    {

        _logger = logger;

        _eventHubKudos = new EventHubConsumerHelper<int>(
                               configuration["EventHub_KudosDeletedConnectionString"],
                               configuration["EventHub_KudosDeletedName"],
                               configuration["EventHub_blobStorageConnectionString"],
                               configuration["EventHub_blobContainerName"]
                               );

        _eventHubKudos.UpdateCallback += (kudos =>
        {
            
                foreach (IObserverEventHub<int> observer in _observers.Values)
                {
                    observer.NotifyUpdate(kudos);
                }
            
        });

        _eventHubKudos.Start().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the exception here
                _logger.LogError(task.Exception.ToString());
                throw task.Exception;
            }
        });

    }
    public void Attach(string userId, IObserverEventHub<int> observer)
    {
        _observers.AddOrUpdate(userId, observer,
                    (_, existingObserver) => existingObserver);
    }

    public void Detach(string userId)
    {

        _observers.TryRemove(userId, out _);
    }
}
