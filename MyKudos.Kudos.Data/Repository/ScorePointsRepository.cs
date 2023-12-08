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

        _context.Entry(score).State = EntityState.Modified;

        return _context.SaveChanges() > 0;
    }
}
