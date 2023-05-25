using MyKudos.Kudos.Data.Context;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;


namespace MyKudos.Kudos.Data.Repository;

public class CommentsRepository : ICommentsRepository
{

    private KudosDbContext _context;

    public CommentsRepository(KudosDbContext context)
    {
        _context = context;
    }

    public int Add(Comments comments)
    {
        _context.Comments.Add(comments);
        _context.SaveChanges();

        return comments.Id;
    }

    public IEnumerable<Comments> GetComments(int kudosId)
    {
        return _context.Comments.Where(c => c.KudosId == kudosId);
    }

    public bool Like(int commentsId, string personId)
    {

        var commentsLikes = _context.CommentsLikes.Where(k => k.CommentsId == commentsId && k.FromPersonId == personId).First();

        //it is already there
        if (commentsLikes != null)
            return true;

        _context.CommentsLikes.Add(
                    new CommentsLikes()
                    {
                        FromPersonId = personId,
                        CommentsId = commentsId
                    });

        
        return _context.SaveChanges() > 0;

    }

    public bool UndoLike(int commentsId, string personId)
    {

        var commentsLikes = _context.CommentsLikes.Where(k => k.CommentsId == commentsId && k.FromPersonId == personId).First();

        //it is already removed
        if (commentsLikes == null)
            return true;


        _context.CommentsLikes.Remove(commentsLikes);
       
        return _context.SaveChanges() > 0;

    }


    public bool Update(Comments comments)
    {
        var comment = _context.Comments.FirstOrDefault(c => c.Id == comments.Id);

        if (comment != null)
        {
            comment.Message = comments.Message;
        }

        return _context.SaveChanges() > 0;
    }

    public bool Delete(int commentId)
    {

        var comment = _context.Comments.FirstOrDefault(c => c.Id == commentId);

        if (comment != null)
        {
            _context.Comments.Remove(comment);
        }

        return _context.SaveChanges() > 0;
    }



}
