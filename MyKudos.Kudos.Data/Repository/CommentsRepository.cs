using MyKudos.Kudos.Data.Context;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;


namespace MyKudos.Kudos.Data.Repository;

public class CommentsRepository : ICommentsRepository
{

    private CommentsDbContext _context;

    public CommentsRepository(CommentsDbContext context)
    {
        _context = context;
    }

    public Guid Add(Comments comments)
    {
        _context.Comments.Add(comments);
        _context.SaveChanges();

        return comments.Id;
    }

    public IEnumerable<Comments> GetComments(string kudosId)
    {
        return _context.Comments.Where(c=> c.KudosId == kudosId);
    }

    public bool Like(string commentsId, string personId)
    {

        var comments = _context.Comments.Where(k => k.KudosId == commentsId).FirstOrDefault();

        if (comments == null)
            return false;

            if (comments.Likes == null)
            {
                comments.Likes = new List<string>();
                comments.Likes.Add(personId);
            }
            else if(!comments.Likes.Contains(personId))
            {
                comments.Likes.Add(personId);
   
            }

        return _context.SaveChanges() > 0;

    }

    public bool UndoLike(string commentsId, string personId)
    {

        var comments = _context.Comments.Where(k => k.KudosId == commentsId).FirstOrDefault();

        if ((comments == null) || (comments.Likes == null))
            return false;

        if (comments.Likes.Contains(personId))
        {
            comments.Likes.Remove(personId);
        }

        return _context.SaveChanges() > 0;

    }


    public bool Update(Comments comments)
    {
        var comment = _context.Comments.FirstOrDefault(c => c.Id == comments.Id);

        if (comment != null)
        {
            comment.Message = comments.Message;
        }

        return _context.SaveChanges() >0;
    }

    public bool Delete(Guid commentId) {

        var comment = _context.Comments.FirstOrDefault(c => c.Id == commentId);

        if (comment != null)
        {
            _context.Comments.Remove(comment);
        }

        return _context.SaveChanges() > 0;
    }
}
