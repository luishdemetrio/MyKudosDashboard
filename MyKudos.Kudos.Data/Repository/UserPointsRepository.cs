
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyKudos.Kudos.Data.Context;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;
using System.Data;

namespace MyKudos.Kudos.Data.Repository;



public class UserPointsRepository : IUserPointsRepository
{

    private KudosDbContext _context;

    private string _allGroupCompletedImage;
    private string _allGroupCompletedDescription;

    public UserPointsRepository(KudosDbContext scoreContext, IConfiguration configuration)
    {
        _context = scoreContext;


        _allGroupCompletedImage = configuration["AllGroupCompletedImage"];
        _allGroupCompletedDescription = configuration["AllGroupCompletedDescription"];

    }

    
    public List<UserPoint> GetTopUserScores(int top)
    {

        var parameter = new SqlParameter("@top", SqlDbType.Int) { Value = top };

        var result = _context.Set<UserPoint>().FromSqlRaw("EXEC SP_GETTOPCONTRIBUTORS @top", parameter).ToList();

        return result;
    }

    public UserPointScore GetUserScore(Guid pUserId)
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




        var subquery = from rt in _context.Recognitions
                       group rt by rt.RecognitionGroupId into g
                       select new
                       {
                           RecognitionGroupId = g.Key,
                           Total = g.Count()
                       };
        int index= 1;

       
        var kudosSentBadges = (from row in _context.BadgeRules
                               where (row.ActionType == "send_kudos") && (
                                                (result.KudosSent >= row.Initial && result.KudosSent <= row.Final) ||
                                                (result.KudosSent >= row.Initial && row.Final == null))
                               select new UserBadge
                               {
                                   BadgeName = row.ImageName,
                                   BadgeDescription = row.Description
                               }).AsEnumerable()
                             .Select (row => new UserBadge
                              {
                                  BadgeName = row.BadgeName,
                                  BadgeDescription = row.BadgeDescription,
                                  UserBadgeId = index++
                              });

        var kudosReceivedBadges =   (from row in _context.BadgeRules
                                    where (row.ActionType == "received_kudos") && (
                                                        (result.KudosReceived >= row.Initial && result.KudosReceived <= row.Final) ||
                                                        (result.KudosReceived >= row.Initial && row.Final == null))
                                    select new UserBadge
                                    {
                                        BadgeName = row.ImageName,
                                        BadgeDescription = row.Description
                                    }).AsEnumerable()
                             .Select(row => new UserBadge
                             {
                                 BadgeName = row.BadgeName,
                                 BadgeDescription = row.BadgeDescription,
                                 UserBadgeId = index++
                             });


        var distinctRecognitionIds = _context.Kudos
                                        .Where(k => k.FromPersonId == Guid.Parse("17afd395-3455-4810-bc72-58eb7eed4c42"))
                                        .Select(k => k.RecognitionId)
                                        .Distinct();

        var badges = (from k in distinctRecognitionIds
                      join r in _context.Recognitions on k equals r.RecognitionId
                      join rg in _context.RecognitionsGroup on r.RecognitionGroupId equals rg.RecognitionGroupId
                      join t in subquery on r.RecognitionGroupId equals t.RecognitionGroupId
                      group k by new { rg.RecognitionGroupId, rg.BadgeName, rg.Description, t.Total } into g
                      where g.Count() >= g.Key.Total
                      select g.Key).AsEnumerable()
                     .Select(g => new UserBadge
                     {
                         BadgeName = g.BadgeName,
                         BadgeDescription = $"Todos os comportamentos: {g.Description}",
                         UserBadgeId = index++
                     });

        result.EarnedBagdes.AddRange(kudosSentBadges.ToList());
        result.EarnedBagdes.AddRange(kudosReceivedBadges.ToList());
        result.EarnedBagdes.AddRange(badges.ToList());

        if (subquery.Count() == badges.Count())
        {
            result.EarnedBagdes.Add(new UserBadge
            {
                BadgeName = _allGroupCompletedImage,
                BadgeDescription = _allGroupCompletedDescription
            });
        }


        return result;

    }

    public bool SetPoints(Points points)
    {
        _context.Points.Add(points);

        return _context.SaveChanges() > 0;
    }
}
