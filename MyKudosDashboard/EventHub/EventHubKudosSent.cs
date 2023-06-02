using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.EventGrid;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace MyKudosDashboard.EventHub;

public class EventHubKudosSent : IEventHubReceived<KudosResponse>
{
    private ConcurrentQueue<IObserverEventHub<KudosResponse>> _observers
                        = new ConcurrentQueue<IObserverEventHub<KudosResponse>>();

    private EventHubConsumerHelper<KudosResponse> _eventHubScore;

    public EventHubKudosSent(IConfiguration configuration)
    {
        _eventHubScore = new EventHubConsumerHelper<KudosResponse>(
                               configuration["EventHub_KudosSentConnectionString"],
                               configuration["EventHub_KudosSentName"],
                               configuration["EventHub_blobStorageConnectionString"],
                               configuration["EventHub_blobContainerName"]
                               );

        _eventHubScore.UpdateCallback += (score => 
            {
                if (score != null)
                {
                    foreach (IObserverEventHub<KudosResponse> observer in _observers)
                    {
                        observer.NotifyUpdate(score);
                    }
                }
            });

        _eventHubScore.Start();
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
