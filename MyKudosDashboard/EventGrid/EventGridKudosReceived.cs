using MyKudos.Gateway.Domain.Models;
using Newtonsoft.Json;

namespace MyKudosDashboard.EventGrid;

public class EventGridKudosReceived : IEventGridKudosReceived
{

    private List<IObserverKudos> _observers = new List<IObserverKudos>();
   
    public void Attach(IObserverKudos observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IObserverKudos observer)
    {
        _observers.Remove(observer);
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
