using SuperKudos.KudosCatalog.Domain.Models;

namespace SuperKudos.Aggregator.Interfaces;

public interface ICommentsService
{
    
    Task<bool> LikeCommentAsync(SendLike like);

    Task<bool> UndoLikeCommentAsync(SendLike dislike);

    Task<int> SendCommentsAsync(Comments comment);

    Task<IEnumerable<Comments>> GetComments(int kudosId);

    Task<bool> UpdateComments(Comments comments);

    Task<bool> DeleteComments(int kudosId, int commentId);

}