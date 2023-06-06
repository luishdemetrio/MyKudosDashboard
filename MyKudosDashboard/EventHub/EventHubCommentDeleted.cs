using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.EventHub.Enums;
using System.Collections.Concurrent;

namespace MyKudosDashboard.EventHub;



public class EventHubCommentDeleted : IEventHubReceived<EventHubResponse<EventHubCommentOptions,CommentsRequest>>
{
    private ConcurrentQueue<IObserverEventHub<EventHubResponse<EventHubCommentOptions, CommentsRequest>>> _observers
                        = new ConcurrentQueue<IObserverEventHub<EventHubResponse<EventHubCommentOptions, CommentsRequest>>>();

    private EventHubConsumerHelper<CommentsRequest> _eventHubScore;
    
    private static EventHubCommentDeleted instance;
    private static object lockObject = new object();

    // Private constructor to prevent instantiation
    private EventHubCommentDeleted()
    {
    }

    public static EventHubCommentDeleted GetInstance(IConfiguration configuration)
    {
        lock (lockObject)
        {
            if (instance == null)
            {
                instance = new EventHubCommentDeleted(configuration);
            }
        }
        return instance;
    }

    private EventHubCommentDeleted(IConfiguration configuration)
    {
        _eventHubScore = new EventHubConsumerHelper<CommentsRequest>(
                               configuration["EventHub_CommentDeletedConnectionString"],
                               configuration["EventHub_CommentDeletedName"],
                               configuration["EventHub_blobStorageConnectionString"],
                               configuration["EventHub_blobContainerName"]
                               );

        _eventHubScore.UpdateCallback += (score => 
            {
                if (score != null)
                {
                    foreach (IObserverEventHub<EventHubResponse<EventHubCommentOptions, CommentsRequest>> observer in _observers)
                    {
                        
                        observer.NotifyUpdate(new EventHubResponse<EventHubCommentOptions, CommentsRequest>
                        {
                            Kind = EventHubCommentOptions.CommentDeleted, Event = score 
                        });
                    }
                }
            });

        _eventHubScore.Start();
    }
    public void Attach(IObserverEventHub<EventHubResponse<EventHubCommentOptions, CommentsRequest>> observer)
    {
        _observers.Enqueue(observer);
    }

    public void Detach(IObserverEventHub<EventHubResponse<EventHubCommentOptions, CommentsRequest>> observer)
    {
        IObserverEventHub<EventHubResponse<EventHubCommentOptions, CommentsRequest>> removedObserver;

        while (!_observers.IsEmpty)
        {
            _observers.TryDequeue(out removedObserver);
            if (removedObserver != observer)
                _observers.Enqueue(removedObserver);
        }
    }

   
}
