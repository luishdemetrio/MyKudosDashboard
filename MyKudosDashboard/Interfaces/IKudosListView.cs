using MyKudos.Gateway.Domain.Models;

namespace MyKudosDashboard.Interfaces;

public interface IKudosListView
{

    Task<bool> SendLikeAsync(LikeGateway like);

    Task<bool> SendUndoLikeAsync(LikeGateway like);

    Task<IEnumerable<KudosResponse>> GetKudos(int pageNumber);
    Task<IEnumerable<KudosResponse>> GetKudosToMe(string userId, int pageNumber);
    Task<IEnumerable<KudosResponse>> GetKudosFromMe(string userId, int pageNumber);
    Task<IEnumerable<KudosResponse>> GetKudosToMyDirectReports(string userId, int pageNumber);



}
