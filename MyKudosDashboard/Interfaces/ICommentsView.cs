using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface ICommentsView
{

    Task<bool> LikeKudosAsync(LikeGateway like);
    Task<bool> UndoLikeKudosAsync(LikeGateway like);

    Task<int> SendComments(CommentsRequest comment);

    Task<IEnumerable<CommentsResponse>> GetComments(int kudosId);

    Task<bool> UpdateComments(CommentsResponse comment, string toPersonId);

    Task<bool> DeleteComments(CommentsResponse comment, string toPersonId);
}
