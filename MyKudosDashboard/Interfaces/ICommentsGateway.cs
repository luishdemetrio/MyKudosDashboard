using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface ICommentsGateway
{

    Task<bool> LikeComment(LikeComment like);

    Task<bool> UndoLikeComment(LikeComment like);

    Task<string> SendCommentsAsync(CommentsRequest comment);

    Task<IEnumerable<CommentsResponse>> GetComments(string kudosId);

    Task<bool> UpdateComments(CommentsRequest comments);

    Task<bool> DeleteComments(CommentsRequest comments);
}
