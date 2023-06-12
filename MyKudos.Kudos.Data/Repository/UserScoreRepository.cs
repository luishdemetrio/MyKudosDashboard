//using MyKudos.Kudos.Data.Context;
//using MyKudos.Kudos.Domain.Interfaces;
//using MyKudos.Kudos.Domain.Models;


//namespace MyKudos.Kudos.Data.Repository;

//public class UserScoreRepository : IUserScoreRepository
//{

//    private KudosDbContext _context;
//    private static SemaphoreSlim _semaphoreLike = new SemaphoreSlim(1, 1);

//    public UserScoreRepository(KudosDbContext scoreContext)
//    {
//        _context = scoreContext;

//    }


//    public UserScore GetUserScore(string pUserId)
//    {
//        UserScore result = new();

//        var score = _context.UserScores?.FirstOrDefault(u => u.UserId == new Guid(pUserId));

//        if (score != null)
//        {
//            result = score;
//        }

//        return result;
//    }


//    public IEnumerable<UserScore> GetTopUserScores(int top)
//    {

//        return _context.UserScores.Where(s => s.Score > 0).OrderByDescending(s => s.Score).Take(top);
//    }

  
//}

