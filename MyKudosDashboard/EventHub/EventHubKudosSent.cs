using MyKudos.Gateway.Domain.Models;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace MyKudosDashboard.EventHub;

public class EventHubKudosSent : IEventHubReceived<KudosResponse>
{
    private ConcurrentQueue<IObserverEventHub<KudosResponse>> _observers
                        = new ConcurrentQueue<IObserverEventHub<KudosResponse>>();

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
                    foreach (IObserverEventHub<KudosResponse> observer in _observers)
                    {
                        observer.NotifyUpdate(kudos);
                    }
                }
            });

        _eventHubKudos.Start();
    }
    public void Attach(IObserverEventHub<KudosResponse> observer)
    {
        _observers.Enqueue(observer);
    }

    public void Detach(IObserverEventHub<KudosResponse> observer)
    {
        IObserverEventHub<KudosResponse> removedObserver;

        while (!_observers.IsEmpty)
        {
            _observers.TryDequeue(out removedObserver);
            if (removedObserver != observer)
                _observers.Enqueue(removedObserver);
        }
    }

   
}
