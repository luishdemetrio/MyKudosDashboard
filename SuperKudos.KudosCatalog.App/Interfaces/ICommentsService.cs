using SuperKudos.KudosCatalog.Domain.Models;


namespace SuperKudos.KudosCatalog.App.Interfaces;

public interface ICommentsService
{

    public bool LikeComment(int kudosId, Guid personId);
    public bool UndoLikeComment(int kudosId, Guid personId);

    int SendComments(Comments comment);

    IEnumerable<Comments> GetComments(int kudosId);

    bool UpdateComments(Comments comments);

    bool DeleteComments(int kudosId, int commentId);

}
