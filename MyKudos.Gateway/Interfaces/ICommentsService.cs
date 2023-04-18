using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Gateway.Interfaces;

public interface ICommentsService
{
    
    Task<bool> LikeCommentAsync(Kudos.Domain.Models.SendLike like);

    Task<bool> UndoLikeCommentAsync(Kudos.Domain.Models.SendLike dislike);

    Task<string> SendCommentsAsync(Comments comment);

    Task<IEnumerable<Comments>> GetComments(string kudosId);

    Task<bool> UpdateComments(Comments comments);

    Task<bool> DeleteComments(string kudosId, string commentId);

}