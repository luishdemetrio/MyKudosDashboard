using Blazorise.Licensing;
using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.EventHub.Enums;
using System.Collections.Concurrent;

namespace MyKudosDashboard.EventHub;



public class EventHubUndoLike : IEventHubUndoLike
{
    private ConcurrentDictionary<string,IObserverEventHub<EventHubResponse<EventHubLikeOptions, LikeGateway>>> _observers
                        = new ();

    private EventHubConsumerHelper<LikeGateway> _eventHub;

    private ILogger<EventHubUndoLike> _logger;  

    public EventHubUndoLike(IConfiguration configuration,
                            ILogger<EventHubUndoLike> logger)
    {
        _logger = logger;

        _eventHub = new EventHubConsumerHelper<LikeGateway>(
                               configuration["EventHub_UndoLikeSentConnectionString"],
                               configuration["EventHub_UndoLikeSentName"],
                               configuration["EventHub_blobStorageConnectionString"],
                               configuration["EventHub_blobContainerName"]
                               );

        _eventHub.UpdateCallback += (score => 
            {
                if (score != null)
                {
                    foreach (IObserverEventHub<EventHubResponse<EventHubLikeOptions, LikeGateway>> observer in _observers.Values)
                    {
                        
                        observer.NotifyUpdate(new EventHubResponse<EventHubLikeOptions, LikeGateway>
                        {
                            Kind = EventHubLikeOptions.UndoLike, Event = score 
                        });
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
    public void Attach(string userId, IObserverEventHub<EventHubResponse<EventHubLikeOptions, LikeGateway>> observer)
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
