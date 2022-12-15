using MyKudos.Dashboard.Domain.Models;

namespace MyKudos.Dashboard.App.Interface;

public interface IKudosService
{

    void Send(Kudos kudos);
}
