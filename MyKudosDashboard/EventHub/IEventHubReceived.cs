﻿using MyKudos.Gateway.Domain.Models;

namespace MyKudosDashboard.EventHub;

public interface IEventHubReceived<T>
{
    void Attach(IObserverEventHub<T> observer);
    void Detach(IObserverEventHub<T> observer);
   // void NotifyUserPointsUpdate(string json);
}