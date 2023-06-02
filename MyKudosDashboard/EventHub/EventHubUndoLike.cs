using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.EventGrid;
using MyKudosDashboard.EventHub.Enums;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace MyKudosDashboard.EventHub;



public class EventHubUndoLike : IEventHubReceived<EventHubResponse<EventHubLikeOptions, LikeGateway>>
{
    private ConcurrentQueue<IObserverEventHub<EventHubResponse<EventHubLikeOptions, LikeGateway>>> _observers
                        = new ConcurrentQueue<IObserverEventHub<EventHubResponse<EventHubLikeOptions, LikeGateway>>>();

    private EventHubConsumerHelper<LikeGateway> _eventHubScore;

    private static EventHubUndoLike instance;
    private static object lockObject = new object();

    // Private constructor to prevent instantiation
    private EventHubUndoLike()
    {
    }

    public static EventHubUndoLike GetInstance(IConfiguration configuration)
    {
        lock (lockObject)
        {
            if (instance == null)
            {
                instance = new EventHubUndoLike(configuration);
            }
        }
        return instance;
    }

    private EventHubUndoLike(IConfiguration configuration)
    {
        _eventHubScore = new EventHubConsumerHelper<LikeGateway>(
                               configuration["EventHub_UndoLikeSentConnectionString"],
                               configuration["EventHub_UndoLikeSentName"]
                               );

        _eventHubScore.UpdateCallback += (score => 
            {
                if (score != null)
                {
                    foreach (IObserverEventHub<EventHubResponse<EventHubLikeOptions, LikeGateway>> observer in _observers)
                    {
                        
                        observer.NotifyUpdate(new EventHubResponse<EventHubLikeOptions, LikeGateway>
                        {
                            Kind = EventHubLikeOptions.UndoLike, Event = score 
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
