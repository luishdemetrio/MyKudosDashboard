using MyKudos.Kudos.Domain.Models;


namespace MyKudos.Kudos.Domain.Interfaces;

public  interface ICommentsRepository
{


    Guid Add(Comments comments);

    int SendLike(string commentsId, string personId);

    IEnumerable<Comments> GetComments(string kudosId);

}
