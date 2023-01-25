
using MyKudos.Domain.Core.Bus;
using MyKudos.Kudos.Domain.Events;
using MyKudos.Kudos.Domain.Interfaces;

namespace MyKudos.Kudos.Domain.EventHandlers;

public class SendKudosEventHandler : IEventHandler<SendKudosCreatedEvent>
{

    private readonly IKudosRepository _kudosRepository;

    //private readonly IAgentNotificationService _agentNotificationService;


    public SendKudosEventHandler(IKudosRepository kudosRepository) //, IAgentNotificationService agentNotificationService)
    {
        _kudosRepository = kudosRepository;
        //_agentNotificationService = agentNotificationService;
    }

    public Task Handle(SendKudosCreatedEvent @event)
    {

        var kudos = new Models.KudosLog()
        {
            FromPersonId = @event.FromPersonId,
            ToPersonId = @event.ToPersonId,
            TitleId = @event.TitleId,
            Message = @event.Message,
            Date = @event.Date
        };

        _kudosRepository.Add(kudos);

       // _agentNotificationService.SendNotification(kudos);

        return Task.CompletedTask;
    }
}
