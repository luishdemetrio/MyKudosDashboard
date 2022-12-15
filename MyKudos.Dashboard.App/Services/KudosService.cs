
using MyKudos.Dashboard.App.Interface;
using MyKudos.Dashboard.Domain.Commands;
using MyKudos.Dashboard.Domain.Models;
using MyKudos.Domain.Core.Bus;

namespace MyKudos.Dashboard.App.Services;

public class KudosService : IKudosService
{

    private readonly IEventBus _bus;

    public KudosService(IEventBus bus)
    {
        _bus = bus;
    }

    public void Send(Kudos kudos)
    {

        var createSendKudosCommand = new CreateSendKudosCommand
            (
                kudos.PersonId,
                kudos.TitleId,
                kudos.Message
            );

        _bus.SendCommand( createSendKudosCommand );

    }
}
