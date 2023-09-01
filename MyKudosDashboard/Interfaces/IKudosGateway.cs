using MyKudos.Gateway.Domain.Models;

namespace MyKudosDashboard.Interfaces;

public interface IKudosGateway
{
    

    Task<string> SendKudos(SendKudosRequest kudos);

    Task<IEnumerable<KudosResponse>> GetKudos(int pageNumber);
    Task<IEnumerable<KudosResponse>> GetKudosFromMe(string userId, int pageNumber);
    Task<IEnumerable<KudosResponse>> GetKudosToMe(string userId, int pageNumber);
    Task<IEnumerable<KudosResponse>> GetKudosToMyDirects(string userId, int pageNumber);


    Task<bool> Like(LikeGateway like);

    Task<bool> UndoLike(LikeGateway like);


}
