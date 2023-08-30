using SuperKudos.Aggregator.Domain.Models;
using System.Collections.Concurrent;

namespace MyKudosDashboard.EventHub;

public class EventHubKudosSent : IEventHubKudosSent
{
    private ConcurrentDictionary<string, IObserverEventHub<KudosResponse>> _observers
                        = new ();

    private EventHubConsumerHelper<KudosResponse> _eventHubKudos;

    private ILogger<EventHubKudosSent> _logger;

    public EventHubKudosSent(IConfiguration configuration, 
                             ILogger<EventHubKudosSent> logger )
    {

        _logger = logger;
        
        _eventHubKudos = new EventHubConsumerHelper<KudosResponse>(
                               configuration["EventHub_KudosSentConnectionString"],
                               configuration["EventHub_KudosSentName"],
                               configuration["EventHub_blobStorageConnectionString"],
                               configuration["EventHub_blobContainerName"]
                               );

        _eventHubKudos.UpdateCallback += (kudos =>
            {
                if (kudos != null)
                {
                    foreach (IObserverEventHub<KudosResponse> observer in _observers.Values)
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
    public void Attach(string userId, IObserverEventHub<KudosResponse> observer)
    {
        _observers.AddOrUpdate(userId, observer, 
                    (_, existingObserver) => existingObserver);
    }

    public void Detach(string userId)
    {
        
        _observers.TryRemove(userId, out _);
    }

   
}
