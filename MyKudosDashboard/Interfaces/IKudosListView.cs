using MyKudos.Gateway.Domain.Models;

namespace MyKudosDashboard.Interfaces;

public interface IKudosListView
{

    Task<bool> SendLikeAsync(int pKudosId, Guid pFromPersonId);

    Task<bool> SendUndoLikeAsync(int pKudosId, Guid pFromPersonId);

    Task<IEnumerable<KudosResponse>> GetKudos(int pageNumber);
    Task<IEnumerable<KudosResponse>> GetKudosToMe(string userId, int pageNumber);
    Task<IEnumerable<KudosResponse>> GetKudosFromMe(string userId, int pageNumber);
    Task<IEnumerable<KudosResponse>> GetKudosToMyDirectReports(string userId, int pageNumber);

    Task<bool> UpdateKudos(KudosMessage kudos);

    Task<bool> DeleteKudos(int kudosId);

}
