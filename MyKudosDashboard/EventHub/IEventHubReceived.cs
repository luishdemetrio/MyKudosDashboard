using MyKudos.Gateway.Domain.Models;

namespace MyKudosDashboard.EventHub;

public interface IEventHubReceived<T>
{
    void Attach(string user, IObserverEventHub<T> observer);
    void Detach(string user);

    //void Detach(IObserverEventHub<T> observer);
    // void NotifyUserPointsUpdate(string json);
}
