using SuperKudos.Aggregator.Domain.Models;
using MyKudosDashboard.EventHub.Enums;
using System.Collections.Concurrent;

namespace MyKudosDashboard.EventHub;

public class EventHubCommentSent : IEventHubCommentSent
{
    private ConcurrentDictionary<string, IObserverEventHub<EventHubResponse<EventHubCommentOptions, CommentsRequest>>> _observers
                        = new ();

    private EventHubConsumerHelper<CommentsRequest> _eventHubScore;
    
    private ILogger<EventHubCommentSent> _logger;


    public EventHubCommentSent(IConfiguration configuration, 
                               ILogger<EventHubCommentSent> logger)
    {

        _logger = logger;
        
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

        _observers.TryRemove(userId, out _);
    }


}
