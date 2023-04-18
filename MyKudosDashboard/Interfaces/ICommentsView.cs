using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface ICommentsView
{

    Task<bool> LikeKudosAsync(Like like);
    Task<bool> UndoLikeKudosAsync(Like like);

    Task<string> SendComments(CommentsRequest comment);

    Task<IEnumerable<CommentsResponse>> GetComments(string kudosId);

    Task<bool> UpdateComments(CommentsResponse comment, string toPersonId);

    Task<bool> DeleteComments(CommentsResponse comment, string toPersonId);
}
