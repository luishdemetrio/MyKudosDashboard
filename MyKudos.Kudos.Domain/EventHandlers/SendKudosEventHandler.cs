
using MyKudos.Domain.Core.Bus;
using MyKudos.Kudos.Domain.Events;
using MyKudos.Kudos.Domain.Interfaces;

namespace MyKudos.Kudos.Domain.EventHandlers;

public class SendKudosEventHandler : IEventHandler<SendKudosCreatedEvent>
{

    private readonly IKudosRepository _kudosRepository;

    public SendKudosEventHandler(IKudosRepository kudosRepository)
    {
        _kudosRepository = kudosRepository;
    }

    public Task Handle(SendKudosCreatedEvent @event)
    {

        _kudosRepository.Add(new Models.KudosLog() { 
            FromPersonId = @event.FromPersonId,
            ToPersonId = @event.ToPersonId,
            TitleId = @event.TitleId,
            Message = @event.Message,
            Date = @event.Date
            });

        return Task.CompletedTask;
    }
}
