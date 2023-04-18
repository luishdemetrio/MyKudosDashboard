using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface IKudosGateway
{
    

    Task<string> SendKudos(KudosRequest kudos);

    Task<IEnumerable<KudosResponse>> GetKudos();


    Task<bool> Like(Like like);

    Task<bool> UndoLike(Like like);


}
