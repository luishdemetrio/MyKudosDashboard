using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.EventGrid;
using MyKudosDashboard.EventHub.Enums;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace MyKudosDashboard.EventHub;



public class EventHubLikeSent : IEventHubReceived<EventHubResponse<EventHubLikeOptions, LikeGateway>>
{
    private ConcurrentQueue<IObserverEventHub<EventHubResponse<EventHubLikeOptions, LikeGateway>>> _observers
                        = new ConcurrentQueue<IObserverEventHub<EventHubResponse<EventHubLikeOptions, LikeGateway>>>();

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
                               configuration["EventHub_LikeSentName"]
                               );

        _eventHubScore.UpdateCallback += (score => 
            {
                if (score != null)
                {
                    foreach (IObserverEventHub<EventHubResponse<EventHubLikeOptions, LikeGateway>> observer in _observers)
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
    public void Attach(IObserverEventHub<EventHubResponse<EventHubLikeOptions, LikeGateway>> observer)
    {
        _observers.Enqueue(observer);
    }

    public void Detach(IObserverEventHub<EventHubResponse<EventHubLikeOptions, LikeGateway>> observer)
    {
        IObserverEventHub<EventHubResponse<EventHubLikeOptions, LikeGateway>> removedObserver;

        while (!_observers.IsEmpty)
        {
            _observers.TryDequeue(out removedObserver);
            if (removedObserver != observer)
                _observers.Enqueue(removedObserver);
        }
    }

   
}
