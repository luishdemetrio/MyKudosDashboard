using MyKudosDashboard.EventHub;

namespace MyKudosDashboard.EventGrid;

public interface IEventGridKudosReceived
{

    void Attach(IObserverKudos observer);
    void Detach(IObserverKudos observer);
   

    void NotifyKudosSentUpdate(string kudos);

    void NotifyLikesSentUpdate(string json);

    void NotifyUndoLikesSentUpdate(string json);

    void NotifyMessageSentUpdate(string json);

    void NotifyMessageDeletedUpdate(string json);

    void NotifyMessageUpdatedUpdate(string json);
}
