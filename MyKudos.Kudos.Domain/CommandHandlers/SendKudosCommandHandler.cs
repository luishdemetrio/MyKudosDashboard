﻿using MediatR;
using MyKudos.Kudos.Domain.Commands;
using MyKudos.Kudos.Domain.Events;
using MyKudos.Domain.Core.Bus;

namespace MyKudos.Kudos.Domain.CommandHandlers;

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

        _bus.Publish(new SendKudosCreatedEvent(request.FromPersonId, request.ToPersonId, request.TitleId, request.Message, request.Date));

        return Task.FromResult(true);
    }
}