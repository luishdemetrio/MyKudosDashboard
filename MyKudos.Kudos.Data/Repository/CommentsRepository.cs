using Microsoft.EntityFrameworkCore;
using MyKudos.Kudos.Data.Context;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;
using System.Xml.Linq;


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

        return comments.CommentsId;
    }

    public IEnumerable<Comments> GetComments(int kudosId)
    {
        var comments =  _context.Comments
                    .Where(c => c.KudosId == kudosId)
                    .Include(c=> c.Likes)
                    .Include(c => c.UserFrom)
        .ToList();

        foreach (var comment in comments)
        {
            foreach (var like in comment.Likes)
            {
                like.Person = _context.UserProfiles.First(u => u.UserProfileId == like.FromPersonId);
            }
        }

        return comments;
    }

    public bool Like(int commentsId, Guid personId)
    {

        var commentsLikes = _context.CommentsLikes.Where(k => k.CommentsId == commentsId && k.FromPersonId == personId).FirstOrDefault();

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

    public bool UndoLike(int commentsId, Guid personId)
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
        var comment = _context.Comments.FirstOrDefault(c => c.CommentsId == comments.CommentsId);

        if (comment != null)
        {
            comment.Message = comments.Message;
        }

        return _context.SaveChanges() > 0;
    }

    public bool Delete(int commentId)
    {

        var comment = _context.Comments.FirstOrDefault(c => c.CommentsId == commentId);

        if (comment != null)
        {
            _context.Comments.Remove(comment);
        }

        return _context.SaveChanges() > 0;
    }



}
