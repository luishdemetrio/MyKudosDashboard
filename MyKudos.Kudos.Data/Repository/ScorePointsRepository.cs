using Microsoft.EntityFrameworkCore;
using MyKudos.Kudos.Data.Context;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Data.Repository;

public class ScorePointsRepository : IScorePointsRepository
{
    private KudosDbContext _context;
        
    public ScorePointsRepository(KudosDbContext kudosDbContext)
    {
        _context = kudosDbContext;
    }

    public IEnumerable<ScorePoints> GetScore()
    {
        return _context.ScorePoints;
    }


    /// <summary>
    /// The ScorePoints is a table that must be just one record.
    /// </summary>
    /// <param name="score"></param>
    /// <returns></returns>
    public bool UpdateScore(ScorePoints score)
    {

        //I need to check if there are any record there in the database

        var scoreInDb = _context.ScorePoints.FirstOrDefault();

        //modify in case there are the score
        if (scoreInDb != null)
        {
            //_context.Entry(score).State = EntityState.Modified;
            scoreInDb.KudosSent = score.KudosSent;
            scoreInDb.KudosReceived = score.KudosReceived;
            scoreInDb.LikesSent = score.LikesSent;
            scoreInDb.LikesReceived = score.LikesReceived;
            scoreInDb.CommentsSent = score.CommentsSent;
            scoreInDb.CommentsReceived = score.CommentsReceived;
        }
        else
        {
            _context.ScorePoints.Add(score);
        }
        

        return _context.SaveChanges() > 0;
    }
}
