using MediatR;
using MyKudos.Dashboard.Domain.Commands;
using MyKudos.Dashboard.Domain.Events;
using MyKudos.Domain.Core.Bus;

namespace MyKudos.Dashboard.Domain.CommandHandlers;

public class SendKudosCommandHandler : IRequestHandler<CreateSendKudosCommand, bool>
{

    private readonly IEventBus _bus;

    public SendKudosCommandHandler(IEventBus bus)
    {
        _bus = bus;
    }

    public Task<bool> Handle(CreateSendKudosCommand request, CancellationToken cancellationToken)
    {

        //publish message to RabbitMQ

        _bus.Publish(new SendKudosCreatedEvent(request.PersonId, request.TitleId, request.Message));

        return Task.FromResult(true);
    }
}
