using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.EventHub.Enums;
using System.Collections.Concurrent;

namespace MyKudosDashboard.EventHub;



public class EventHubCommentSent : IEventHubReceived<EventHubResponse<EventHubCommentOptions,CommentsRequest>>
{
    private ConcurrentDictionary<string, IObserverEventHub<EventHubResponse<EventHubCommentOptions, CommentsRequest>>> _observers
                        = new ();

    private EventHubConsumerHelper<CommentsRequest> _eventHubScore;
    
    private static EventHubCommentSent instance;
    private static object lockObject = new object();

    // Private constructor to prevent instantiation
    private EventHubCommentSent()
    {
    }

    public static EventHubCommentSent GetInstance(IConfiguration configuration)
    {
        lock (lockObject)
        {
            if (instance == null)
            {
                instance = new EventHubCommentSent(configuration);
            }
        }
        return instance;
    }

    private EventHubCommentSent(IConfiguration configuration)
    {
        _eventHubScore = new EventHubConsumerHelper<CommentsRequest>(
                               configuration["EventHub_CommentSentConnectionString"],
                               configuration["EventHub_CommentSentName"],
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
                            Kind = EventHubCommentOptions.CommentSent, Event = score 
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
