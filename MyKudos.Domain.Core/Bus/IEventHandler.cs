using MyKudos.Domain.Core.Events;


namespace MyKudos.Domain.Core.Bus;

public interface IEventHandler<in TEvent> : IEventHandler
      where TEvent : Event
{

    Task Handle(TEvent @event);

}

public interface IEventHandler
{

}
