using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.EventHub;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;

namespace MyKudosDashboard.EventGrid;

public class EventGridKudosReceived : IEventGridKudosReceived
{

    private ConcurrentQueue<IObserverKudos> _observers = new ConcurrentQueue<IObserverKudos>();
   
    public void Attach(IObserverKudos observer)
    {
        _observers.Enqueue(observer);
    }

    public void Detach(IObserverKudos observer)
    {
        IObserverKudos removedObserver;

        while (!_observers.IsEmpty)
        {
            _observers.TryDequeue(out removedObserver);
            if (removedObserver != observer)
                _observers.Enqueue(removedObserver);
        }

       
    }

    public void NotifyKudosSentUpdate(string json)
    {
            
        var kudos = JsonConvert.DeserializeObject<KudosResponse>(json);

        if (kudos != null)
        {
            foreach (IObserverKudos observer in _observers)
            {
                observer.UpdateKudosSent(kudos);
            }
        }
            
    }

    public void NotifyLikesSentUpdate(string json)
    {

        var likes = JsonConvert.DeserializeObject<LikeGateway>(json);

        if (likes != null)
        {
            foreach (IObserverKudos observer in _observers)
            {
                observer.UpdateLikeSent(likes);
            }
        }
            
    }

    public void NotifyUndoLikesSentUpdate(string json)
    {
        var likes = JsonConvert.DeserializeObject<LikeGateway>(json);

        if (likes != null)
        {
            foreach (IObserverKudos observer in _observers)
            {
                observer.UpdateUndoLikeSent(likes);
            }
        }
    }

    public void NotifyMessageDeletedUpdate(string json)
    {
        var comments = JsonConvert.DeserializeObject<CommentsRequest>(json);

        if (comments != null)
        {
            foreach (IObserverKudos observer in _observers)
            {
                observer.UpdateMessageDeleted(comments);
            }
        }
    }

    public void NotifyMessageSentUpdate(string json)
    {
        var comments = JsonConvert.DeserializeObject<CommentsRequest>(json);

        if (comments != null)
        {
            foreach (IObserverKudos observer in _observers)
            {
                observer.UpdateMessageSent(comments);
            }
        }
    }

    public void NotifyMessageUpdatedUpdate(string json)
    {
        var comments = JsonConvert.DeserializeObject<CommentsRequest>(json);

        if (comments != null)
        {
            foreach (IObserverKudos observer in _observers)
            {
                observer.UpdateMessageUpdated(comments);
            }
        }
    }


}
