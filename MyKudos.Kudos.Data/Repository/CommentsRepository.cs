﻿using MyKudos.Kudos.Data.Context;
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

    //return -1 means that the user unliked
    //return 1 means that the user liked
    public int SendLike(string commentsId, string personId)
    {

        int sign = 0;

        var comments = _context.Comments.Where(k => k.Id == new Guid(commentsId)).First();

        if (comments != null)
        {
            if (comments.Likes == null)
            {
                comments.Likes = new List<string>();
                comments.Likes.Add(personId);
                sign = 1;
            }
            else if (comments.Likes.Contains(personId))
            {
                comments.Likes.Remove(personId);
                sign = -1;
            }
            else
            {
                comments.Likes.Add(personId);
                sign = 1;
            }

            _context.SaveChanges();
        }

        return sign;

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