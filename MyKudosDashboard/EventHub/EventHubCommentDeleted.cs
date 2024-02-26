using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.EventHub.Enums;
using System.Collections.Concurrent;

namespace MyKudosDashboard.EventHub;



public class EventHubCommentDeleted : IEventHubCommentDeleted
{
    private ConcurrentDictionary<string, IObserverEventHub<EventHubResponse<EventHubCommentOptions, CommentsRequest>>> _observers
                        = new ();

    private EventHubConsumerHelper<CommentsRequest> _eventHubScore;

    private ILogger<EventHubCommentDeleted> _logger;


    public EventHubCommentDeleted(IConfiguration configuration,
                                  ILogger<EventHubCommentDeleted> logger)
    {   
        _logger = logger;

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

        _eventHubScore.Start().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the exception here
                _logger.LogError(task.Exception.ToString());
                throw task.Exception;
            }
        });
    }
    public void Attach(string userId, IObserverEventHub<EventHubResponse<EventHubCommentOptions, CommentsRequest>> observer)
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
