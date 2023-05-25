using MyKudos.Kudos.Domain.Models;


namespace MyKudos.Kudos.App.Interfaces;

public interface ICommentsService
{

    public bool LikeComment(int kudosId, string personId);
    public bool UndoLikeComment(int kudosId, string personId);

    int SendComments(Comments comment);

    IEnumerable<Comments> GetComments(int kudosId);

    bool UpdateComments(Comments comments);

    bool DeleteComments(int kudosId, int commentId);

}
