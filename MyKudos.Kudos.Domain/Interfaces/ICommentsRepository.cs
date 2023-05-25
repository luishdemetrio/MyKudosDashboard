using MyKudos.Kudos.Domain.Models;


namespace MyKudos.Kudos.Domain.Interfaces;

public  interface ICommentsRepository
{


    int Add(Comments comments);

    bool Like(int commentsId, string personId);

    bool UndoLike(int commentsId, string personId);

    IEnumerable<Comments> GetComments(int kudosId);

    bool Update(Comments comments);
    
    bool Delete(int commentId);



}
