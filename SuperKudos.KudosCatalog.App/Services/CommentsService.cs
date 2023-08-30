using SuperKudos.KudosCatalog.App.Interfaces;
using SuperKudos.KudosCatalog.Domain.Interfaces;
using SuperKudos.KudosCatalog.Domain.Models;


namespace SuperKudos.KudosCatalog.App.Services;

public class CommentsService : ICommentsService
{

    private readonly ICommentsRepository _commentsRepository;

    public CommentsService(ICommentsRepository commentsRepository)
    {
        _commentsRepository = commentsRepository;
    }

    public int SendComments(Comments comment)
    {
        var commentsId = _commentsRepository.Add(new Comments()
        {
            KudosId = comment.KudosId,
            FromPersonId = comment.FromPersonId,
            Message = comment.Message,
            Date = comment.Date
        });

        return commentsId;
    }

    public IEnumerable<Comments> GetComments(int kudosId)
    {
        return _commentsRepository.GetComments(kudosId);
    }

    public bool UpdateComments(Comments comments)
    {
        return _commentsRepository.Update(comments);
    }

    public bool DeleteComments(int kudosId, int commentId)
    {

        return _commentsRepository.Delete(commentId);
        
    }

    public bool LikeComment(int kudosId, Guid personId)
    {
        return _commentsRepository.Like(kudosId, personId);
    }

    public bool UndoLikeComment(int kudosId, Guid personId)
    {
        return _commentsRepository.UndoLike(kudosId, personId);
    }
}
