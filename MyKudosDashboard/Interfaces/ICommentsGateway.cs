using SuperKudos.Aggregator.Domain.Models;

namespace MyKudosDashboard.Interfaces;

public interface ICommentsGateway
{

    Task<bool> LikeComment(LikeCommentGateway like);

    Task<bool> UndoLikeComment(LikeCommentGateway like);

    Task<int> SendCommentsAsync(CommentsRequest comment);

    Task<IEnumerable<CommentsResponse>> GetComments(int kudosId);

    Task<bool> UpdateComments(CommentsRequest comments);

    Task<bool> DeleteComments(CommentsRequest comments);
}
