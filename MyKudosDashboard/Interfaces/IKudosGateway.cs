using MyKudos.Gateway.Domain.Models;

namespace MyKudosDashboard.Interfaces;

public interface IKudosGateway
{   
    Task<string> SendKudos(SendKudosRequest kudos);

    Task<IEnumerable<KudosResponse>> GetKudos(int pageNumber, string userManagerId);
    Task<IEnumerable<KudosResponse>> GetKudosFromMe(string userId, int pageNumber, string userManagerId);
    Task<IEnumerable<KudosResponse>> GetKudosToMe(string userId, int pageNumber, string userManagerId);
    
    Task<bool> Like(SendLikeGateway like);

    Task<bool> UndoLike(SendLikeGateway like);

    Task<bool> UpdateKudos(KudosMessage kudos);

    Task<bool> DeleteKudos(int kudosId);


}
