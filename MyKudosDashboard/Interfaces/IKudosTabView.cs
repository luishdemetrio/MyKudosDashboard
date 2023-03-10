using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface IKudosTabView
{

    Task<IEnumerable<KudosResponse>> GetKudos();


}
