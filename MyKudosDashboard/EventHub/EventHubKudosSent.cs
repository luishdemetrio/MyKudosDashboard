using MyKudos.Gateway.Domain.Models;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace MyKudosDashboard.EventHub;

public class EventHubKudosSent : IEventHubReceived<KudosResponse>
{
    private ConcurrentDictionary<string, IObserverEventHub<KudosResponse>> _observers
                        = new ();

    private EventHubConsumerHelper<KudosResponse> _eventHubKudos;

    private static EventHubKudosSent instance;
    private static object lockObject = new object();

    // Private constructor to prevent instantiation
    private EventHubKudosSent()
    {
    }

    public static EventHubKudosSent GetInstance(IConfiguration configuration)
    {
        lock (lockObject)
        {
            if (instance == null)
            {
                instance = new EventHubKudosSent(configuration);
            }
        }
        return instance;
    }


    private EventHubKudosSent(IConfiguration configuration)
    {
        _eventHubKudos = new EventHubConsumerHelper<KudosResponse>(
                               configuration["EventHub_KudosSentConnectionString"],
                               configuration["EventHub_KudosSentName"],
                               configuration["EventHub_blobStorageConnectionString"],
                               configuration["EventHub_blobContainerName"]
                               );

        _eventHubKudos.UpdateCallback += (kudos => 
            {
                if (kudos != null)
                {
                    foreach (IObserverEventHub<KudosResponse> observer in _observers.Values)
                    {
                        observer.NotifyUpdate(kudos);
                    }
                }
            });

        _eventHubKudos.Start();
    }
    public void Attach(string userId, IObserverEventHub<KudosResponse> observer)
    {
        _observers.AddOrUpdate(userId, observer, 
                    (_, existingObserver) => existingObserver);
    }

    public void Detach(string userId)
    {
        
        _observers.TryRemove(userId, out _);
    }

   
}
