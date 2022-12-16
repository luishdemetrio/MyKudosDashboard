using MyKudos.Dashboard.Domain.Models;

namespace MyKudos.Dashboard.App.Interfaces;

public interface IDashboardKudosService
{

    void Send(KudosLog kudos);
}
