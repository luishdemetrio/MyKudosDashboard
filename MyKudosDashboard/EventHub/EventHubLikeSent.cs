using Microsoft.Bot.Connector.Authentication;
using SuperKudos.Aggregator.Domain.Models;
using MyKudosDashboard.EventHub.Enums;
using System.Collections.Concurrent;

namespace MyKudosDashboard.EventHub;



public class EventHubLikeSent : IEventHubLikeSent//IEventHubReceived<EventHubResponse<EventHubLikeOptions, LikeGateway>>
{
    private ConcurrentDictionary<string, IObserverEventHub<EventHubResponse<EventHubLikeOptions, LikeGateway>>> _observers
                        = new ();

    private EventHubConsumerHelper<LikeGateway> _eventHubScore;
   
    private ILogger<EventHubLikeSent> _logger;

    public EventHubLikeSent(IConfiguration configuration, ILogger<EventHubLikeSent> logger)
    {
        _logger = logger;

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
                        Kind = EventHubLikeOptions.LikeSent,
                        Event = score
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

    //public static EventHubLikeSent GetInstance(IConfiguration configuration)
    //{
    //    lock (lockObject)
    //    {
    //        if (instance == null)
    //        {
    //            instance = new EventHubLikeSent(configuration);
    //        }
    //    }
    //    return instance;
    //}

    //private EventHubLikeSent(IConfiguration configuration)
    //{
    //    _eventHubScore = new EventHubConsumerHelper<LikeGateway>(
    //                           configuration["EventHub_LikeSentConnectionString"],
    //                           configuration["EventHub_LikeSentName"],
    //                           configuration["EventHub_blobStorageConnectionString"],
    //                           configuration["EventHub_blobContainerName"]
    //                           );

    //    _eventHubScore.UpdateCallback += (score => 
    //        {
    //            if (score != null)
    //            {
    //                foreach (IObserverEventHub<EventHubResponse<EventHubLikeOptions, LikeGateway>> observer in _observers.Values)
    //                {
                        
    //                    observer.NotifyUpdate(new EventHubResponse<EventHubLikeOptions, LikeGateway>
    //                    {
    //                        Kind = EventHubLikeOptions.LikeSent, Event = score 
    //                    });
    //                }
    //            }
    //        });


    //    _eventHubScore.Start().ContinueWith(task =>
    //    {
    //        if (task.IsFaulted)
    //        {
    //            // Handle the exception here
    //            MessageCallback?.Invoke(task.Exception.ToString());
    //        }
    //    });
    //}
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
