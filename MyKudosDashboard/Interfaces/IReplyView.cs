using SuperKudos.Aggregator.Domain.Models;

namespace MyKudosDashboard.Interfaces;

public interface IReplyView
{
    Task<bool> SendLikeAsync(LikeCommentGateway like);
    Task<bool> SendUndoLikeAsync(LikeCommentGateway like);

}
