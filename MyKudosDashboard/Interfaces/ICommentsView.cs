using MyKudos.Gateway.Domain.Models;

namespace MyKudosDashboard.Interfaces;

public interface ICommentsView
{

    Task<bool> LikeKudosAsync(LikeGateway like);
    Task<bool> UndoLikeKudosAsync(LikeGateway like);

    Task<int> SendComments(CommentsRequest comment);

    Task<IEnumerable<CommentsResponse>> GetComments(int kudosId);

    Task<bool> UpdateComments(CommentsResponse comment);

    Task<bool> DeleteComments(CommentsResponse comment);
}
