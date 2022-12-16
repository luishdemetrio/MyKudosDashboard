
using MyKudos.Dashboard.App.Interfaces;
using MyKudos.Dashboard.Domain.Commands;
using MyKudos.Dashboard.Domain.Models;
using MyKudos.Domain.Core.Bus;

namespace MyKudos.Dashboard.App.Services;

public class DashboardKudosService : IDashboardKudosService
{

    private readonly IEventBus _bus;

    public DashboardKudosService(IEventBus bus)
    {
        _bus = bus;
    }

    public void Send(KudosLog kudos)
    {

        var createSendKudosCommand = new CreateSendKudosCommand
            (
                kudos.FromPersonId,
                kudos.ToPersonId,
                kudos.TitleId,
                kudos.Message,
                kudos.Date
            );

        _bus.SendCommand( createSendKudosCommand );

    }
}
