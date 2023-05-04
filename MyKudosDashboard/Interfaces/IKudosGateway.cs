using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface IKudosGateway
{
    

    Task<string> SendKudos(KudosRequest kudos);

    Task<IEnumerable<KudosResponse>> GetKudos(int pageNumber);


    Task<bool> Like(Like like);

    Task<bool> UndoLike(Like like);


}
