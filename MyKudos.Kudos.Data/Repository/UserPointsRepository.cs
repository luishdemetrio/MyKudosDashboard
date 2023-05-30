
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyKudos.Kudos.Data.Context;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;
using System.Data;
using System.Linq;

namespace MyKudos.Kudos.Data.Repository;



public class UserPointsRepository : IUserPointsRepository
{

    private KudosDbContext _context;


    public UserPointsRepository(KudosDbContext scoreContext)
    {
        _context = scoreContext;

    }

    
    public List<UserPoint> GetTopUserScores(int top)
    {

        var parameter = new SqlParameter("@top", SqlDbType.Int) { Value = top };

        var result = _context.Set<UserPoint>().FromSqlRaw("EXEC SP_GETTOPCONTRIBUTORS @top", parameter).ToList();

        return result;
    }

    public UserPointScore GetUserScore(string pUserId)
    {

        UserPointScore result = new();

        var pointsPerAction = _context.Points.ToDictionary(p => p.ActionType, p => p.Score);


        result.UserId = pUserId;
        result.KudosSent = _context.Kudos.Count(k => k.FromPersonId == pUserId);
        result.KudosReceived = _context.Kudos.Count(k => k.ToPersonId == pUserId);

        result.LikesSent = _context.KudosLike.Count(l => l.PersonId == pUserId) ;
        result.LikesReceived  = _context.Kudos.Where(k => k.ToPersonId == pUserId).Join(_context.KudosLike, k => k.KudosId, l => l.KudosId, (k, l) => l).Count() ;

        result.MessagesSent = _context.Comments.Count(r => r.FromPersonId == pUserId) ;
        result.MessagesReceived = _context.Kudos.Where(k => k.ToPersonId == pUserId).Join(_context.Comments, k => k.KudosId, r => r.KudosId, (k, r) => r).Count();

        result.Score = (result.KudosSent * pointsPerAction["send_kudos"] ) +
                       (result.KudosReceived * pointsPerAction["receive_kudos"]) + 
                       (result.LikesSent * pointsPerAction["send_like"] ) + 
                       (result.LikesReceived * pointsPerAction["receive_like"]) +
                       (result.MessagesSent * pointsPerAction["send_reply"]) + 
                       (result.MessagesReceived * pointsPerAction["receive_reply"]);


        return result;

    }

    public bool SetPoints(Points points)
    {
        _context.Points.Add(points);

        return _context.SaveChanges() > 0;
    }
}
