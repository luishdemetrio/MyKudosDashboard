using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.EventHub;
using Newtonsoft.Json;


namespace MyKudosDashboard.EventGrid;

public class EventGridUserPointsReceived : IEventGridUserPointsReceived
{
    private List<IObserverUserPoints> _observers = new List<IObserverUserPoints>();

    public void Attach(IObserverUserPoints observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IObserverUserPoints observer)
    {
        _observers.Remove(observer);
    }

    public void NotifyUserPointsUpdate(string json)
    {
        var score = JsonConvert.DeserializeObject<UserPointScore>(json);

        if (score != null)
        {
            foreach (IObserverUserPoints observer in _observers)
            {
                observer.UpdateUserScore(score);
            }
        }
    }
}
