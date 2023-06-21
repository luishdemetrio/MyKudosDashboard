using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.EventHub.Enums;
using System.Collections.Concurrent;

namespace MyKudosDashboard.EventHub;



public class EventHubLikeSent : IEventHubReceived<EventHubResponse<EventHubLikeOptions, LikeGateway>>
{
    private ConcurrentDictionary<string, IObserverEventHub<EventHubResponse<EventHubLikeOptions, LikeGateway>>> _observers
                        = new ();

    private EventHubConsumerHelper<LikeGateway> _eventHubScore;
   
    private static EventHubLikeSent instance;
    private static object lockObject = new object();

    // Private constructor to prevent instantiation
    private EventHubLikeSent()
    {
    }

    public static EventHubLikeSent GetInstance(IConfiguration configuration)
    {
        lock (lockObject)
        {
            if (instance == null)
            {
                instance = new EventHubLikeSent(configuration);
            }
        }
        return instance;
    }

    private EventHubLikeSent(IConfiguration configuration)
    {
        _eventHubScore = new EventHubConsumerHelper<LikeGateway>(
                               configuration["EventHub_LikeSentConnectionString"],
                               configuration["EventHub_LikeSentName"],
                               configuration["EventHub_blobStorageConnectionString"],
                               configuration["EventHub_blobContainerName"]
                               );

        _eventHubScore.UpdateCallback += (score => 
            {
                if (score != null)
                {
                    foreach (IObserverEventHub<EventHubResponse<EventHubLikeOptions, LikeGateway>> observer in _observers.Values)
                    {
                        
                        observer.NotifyUpdate(new EventHubResponse<EventHubLikeOptions, LikeGateway>
                        {
                            Kind = EventHubLikeOptions.LikeSent, Event = score 
                        });
                    }
                }
            });

        _eventHubScore.Start();
    }
    public void Attach(string userId, IObserverEventHub<EventHubResponse<EventHubLikeOptions, LikeGateway>> observer)
    {
        _observers.AddOrUpdate(userId, observer,
                    (_, existingObserver) => existingObserver);
    }

    public void Detach(string userId)
    {

        _observers.TryRemove(userId, out _);
    }


}
