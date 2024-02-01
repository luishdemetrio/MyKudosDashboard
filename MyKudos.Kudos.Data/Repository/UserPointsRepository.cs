
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

        _allGroupCompletedImage = configuration["AllGroupCompletedImage"] ?? 
                                throw new NullReferenceException("AllGroupCompletedImage setting missing");

        _allGroupCompletedDescription = configuration["AllGroupCompletedDescription"] 
                                ?? throw new NullReferenceException("AllGroupCompletedDescription setting missing");

    }

    
    public List<UserPoint> GetTopUserScores(int top, Guid? managerId)
    {

        var parameterTop = new SqlParameter("@top", SqlDbType.Int) { Value = top };

        var parameterManagerId = new SqlParameter("@ManagerId", SqlDbType.UniqueIdentifier) { Value = managerId is null ? DBNull.Value : managerId };
        
        var result = _context.Set<UserPoint>().FromSqlRaw(
                            "EXEC SP_GETTOPCONTRIBUTORS @top, @ManagerId", 
                            parameterTop,
                            parameterManagerId).ToList();

        return result;
    }

    public UserPointScore GetUserScore(Guid pUserId, bool justMyTeam = false)
    {
        UserPointScore result = new();

        //get the table with the points definition
        var pointsPerAction = _context.ScorePoints.First();

        //populate the result with the userid
        result.UserId = pUserId;

        IQueryable<Domain.Models.Kudos> kudosQuery = _context.Kudos
            .Include(c => c.Comments)
            .Include(u => u.UserFrom)
            .Include(u => u.Recognition);

        kudosQuery = kudosQuery.Include(r => r.Recognized).ThenInclude(p => p.Person);
        kudosQuery = kudosQuery.Include(r => r.Likes).ThenInclude(p => p.Person);

        IQueryable<Domain.Models.Comments> commentsQuery = _context.Comments
            .Include(u => u.UserFrom);

        IQueryable<Domain.Models.KudosLike> likesQuery = _context.KudosLike
            .Include(u => u.Person);

        //justMyTeam is true only for managers. When it is true, it means that the pUserId is
        //the id of the manager 
        if (justMyTeam)
        {
            kudosQuery = kudosQuery.Where(k => ( (k.UserFrom.ManagerId == pUserId) || 
                                                 (k.Recognized.Any(u => u.Person.ManagerId == pUserId))
                                               ) || 
                                               ( (k.UserFrom.ManagerId.HasValue == false) &&
                                                 (k.UserFrom.UserProfileId == pUserId)
                                                  ) 
                                               );

          //  commentsQuery = commentsQuery.Where(c => c.UserFrom.ManagerId == pUserId);

//            likesQuery = likesQuery.Where(c => c.Person.ManagerId == pUserId) ;
        }       

        result.KudosSent = kudosQuery.Count(k => k.FromPersonId == pUserId);
        result.KudosReceived = kudosQuery.Count(k => k.Recognized.Any(u => u.ToPersonId == pUserId));

        
        result.LikesSent = likesQuery.Where(l => l.PersonId == pUserId)
                            .Join(kudosQuery, k => k.KudosId, l => l.KudosId, (k, l) => l).Count();

        result.LikesReceived  = kudosQuery.Where(k => k.Recognized.Any(u => u.ToPersonId == pUserId))
                                .Join(likesQuery, k => k.KudosId, l => l.KudosId, (k, l) => l)
                                .Count() ;
        //.Where(w => justMyTeam ? w.Person.ManagerId == pUserId: true)

        result.MessagesSent = commentsQuery.Where(r => r.FromPersonId == pUserId)
                                .Join(kudosQuery, k => k.KudosId, r => r.KudosId, (k, r) => r).Count();

        result.MessagesReceived = kudosQuery.Where(k => k.Recognized.Any(u => u.ToPersonId == pUserId))
                            .Join(commentsQuery, k => k.KudosId, r => r.KudosId, (k, r) => r).Count();

        result.Score = (result.KudosSent * pointsPerAction.KudosSent ) +
                       (result.KudosReceived * pointsPerAction.KudosReceived) + 
                       (result.LikesSent * pointsPerAction.LikesSent ) + 
                       (result.LikesReceived * pointsPerAction.LikesReceived) +
                       (result.MessagesSent * pointsPerAction.CommentsSent) + 
                       (result.MessagesReceived * pointsPerAction.CommentsReceived);


      

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
                                        .Where(k => k.Recognized.Any(u => u.ToPersonId == pUserId))
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

    //public bool SetPoints(Points points)
    //{
    //    _context.Points.Add(points);

    //    return _context.SaveChanges() > 0;
    //}

    //public IEnumerable<Points> GetPoints()
    //{
    //    return _context.Points;
    //}


}
