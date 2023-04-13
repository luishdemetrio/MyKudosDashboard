using MyKudos.Kudos.Domain.Models;


namespace MyKudos.Kudos.Domain.Interfaces;

public  interface ICommentsRepository
{


    Guid Add(Comments comments);

    bool Like(string commentsId, string personId);

    bool UndoLike(string commentsId, string personId);

    IEnumerable<Comments> GetComments(string kudosId);

    bool Update(Comments comments);
    
    bool Delete(Guid commentId);



}
