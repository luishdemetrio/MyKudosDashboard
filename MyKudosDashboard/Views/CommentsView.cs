using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.Interfaces;

namespace MyKudosDashboard.Views;

public class CommentsView : ICommentsView
{

    
    private ICommentsGateway _commentsGateway;

    private IKudosGateway _kudosGateway;

    public CommentsView(ICommentsGateway commentsGateway, IKudosGateway kudosGateway)
    {
        _commentsGateway = commentsGateway;
        _kudosGateway = kudosGateway;

    }

    public Task<bool> LikeKudosAsync(LikeGateway like)
    {
        return _kudosGateway.Like(like);
    }

    public async Task<bool> UndoLikeKudosAsync(LikeGateway like)
    {
        return await _kudosGateway.UndoLike(like);
    }

    public Task<IEnumerable<CommentsResponse>> GetComments(int kudosId)
    {
        return _commentsGateway.GetComments(kudosId);
    }

    public Task<int> SendComments(CommentsRequest comment)
    {
        return _commentsGateway.SendCommentsAsync(comment); 
    }

    public Task<bool> UpdateComments(CommentsResponse comment, string toPersonId)
    {
        return _commentsGateway.UpdateComments(new CommentsRequest()
        {
            Id = comment.Id,
            Date = comment.Date,
            Message = comment.Message,
            KudosId = comment.KudosId,
            FromPersonId = comment.FromPerson.Id,
            ToPersonId = toPersonId
        });
    }

    public  Task<bool> DeleteComments(CommentsResponse comment, string toPersonId)
    {
        return _commentsGateway.DeleteComments(new CommentsRequest()
        {
            Id = comment.Id,
            Date = comment.Date,
            Message = comment.Message,
            KudosId = comment.KudosId,
            FromPersonId = comment.FromPerson.Id,
            ToPersonId = toPersonId
        });
    }
}
