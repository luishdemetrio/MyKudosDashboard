using MyKudosDashboard.EventHub;

namespace MyKudosDashboard.EventGrid;

public interface IEventGridUserPointsReceived
{
    void Attach(IObserverUserPoints observer);
    void Detach(IObserverUserPoints observer);


    void NotifyUserPointsUpdate(string json);
}
