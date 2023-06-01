using MyKudos.Gateway.Domain.Models;

namespace MyKudosDashboard.EventGrid;

public interface IObserverKudos
{
    void UpdateLikeSent(LikeGateway like);
    void UpdateUndoLikeSent(LikeGateway like);

    void UpdateKudosSent(KudosResponse kudos);

    void UpdateMessageSent(CommentsRequest comments);
    void UpdateMessageDeleted(CommentsRequest comments);

    void UpdateMessageUpdated(CommentsRequest comments);

}


public interface IObserverUserPoints
{
    void UpdateUserScore(UserPointScore score);


}


// ISubject interface
public interface ISubjectKudos
{
    void Attach(IObserverKudos observer);
    void Detach(IObserverKudos observer);
    void Notify();
}

public interface ISubjectUserPoints
{
    void Attach(IObserverUserPoints observer);
    void Detach(IObserverUserPoints observer);
    void Notify();
}