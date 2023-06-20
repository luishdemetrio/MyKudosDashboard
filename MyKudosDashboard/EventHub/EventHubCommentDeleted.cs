using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.EventHub.Enums;
using System.Collections.Concurrent;

namespace MyKudosDashboard.EventHub;



public class EventHubCommentDeleted : IEventHubReceived<EventHubResponse<EventHubCommentOptions,CommentsRequest>>
{
    private ConcurrentDictionary<string, IObserverEventHub<EventHubResponse<EventHubCommentOptions, CommentsRequest>>> _observers
                        = new ();

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
                    foreach (IObserverEventHub<EventHubResponse<EventHubCommentOptions, CommentsRequest>> observer in _observers.Values)
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
    public void Attach(string userId, IObserverEventHub<EventHubResponse<EventHubCommentOptions, CommentsRequest>> observer)
    {
        _observers.AddOrUpdate(userId, observer,
                    (_, existingObserver) => existingObserver);
    }

    public void Detach(string userId)
    {

        _observers.TryRemove(userId, out _);
    }


}
