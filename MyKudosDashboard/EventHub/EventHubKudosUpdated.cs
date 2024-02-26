using MyKudos.Gateway.Domain.Models;
using System.Collections.Concurrent;

namespace MyKudosDashboard.EventHub;

public class EventHubKudosUpdated : IEventHubKudosUpdated
{
    private ConcurrentDictionary<string, IObserverEventHub<KudosMessage>> _observers
                         = new();

    private EventHubConsumerHelper<KudosMessage> _eventHubKudos;

    private ILogger<EventHubKudosUpdated> _logger;

    public EventHubKudosUpdated(IConfiguration configuration,
                             ILogger<EventHubKudosUpdated> logger)
    {

        _logger = logger;

        _eventHubKudos = new EventHubConsumerHelper<KudosMessage>(
                               configuration["EventHub_KudosUpdatedConnectionString"],
                               configuration["EventHub_KudosUpdatedName"],
                               configuration["EventHub_blobStorageConnectionString"],
                               configuration["EventHub_blobContainerName"]
                               );

        _eventHubKudos.UpdateCallback += (kudos =>
        {

            if (kudos != null)
            {
                foreach (IObserverEventHub<KudosMessage> observer in _observers.Values)
                {
                    observer.NotifyUpdate(kudos);
                }
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
    public void Attach(string userId, IObserverEventHub<KudosMessage> observer)
    {
        _observers.AddOrUpdate(userId, observer,
                    (_, existingObserver) => existingObserver);
    }

    public void Detach(string userId)
    {
        if (userId != null)
            _observers.TryRemove(userId, out _);
    }
}
