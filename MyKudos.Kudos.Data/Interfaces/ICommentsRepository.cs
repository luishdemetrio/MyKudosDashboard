using MyKudos.Kudos.Domain.Models;


namespace MyKudos.Kudos.Data.Interfaces;

public  interface ICommentsRepository
{


    int Add(Comments comments);

    bool Like(int commentsId, Guid personId);

    bool UndoLike(int commentsId, Guid personId);

    IEnumerable<Comments> GetComments(int kudosId);

    bool Update(Comments comments);
    
    bool Delete(int commentId);



}
