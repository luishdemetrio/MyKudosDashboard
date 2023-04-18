using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Views;

public class ReplyView : IReplyView
{

    private ICommentsGateway _commentsGateway;

    public ReplyView(ICommentsGateway commentsGateway)
    {

        _commentsGateway = commentsGateway;

    }

    public Task<bool> SendLikeAsync(LikeComment like)
    {
        return _commentsGateway.LikeComment(like);
    }

    public Task<bool> SendUndoLikeAsync(LikeComment like)
    {
        return _commentsGateway.UndoLikeComment(like);
    }
}
