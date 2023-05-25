using Microsoft.EntityFrameworkCore;
using MyKudos.Gamification.Data.Context;
using MyKudos.Gamification.Domain.Intefaces;
using MyKudos.Gamification.Domain.Models;

namespace MyKudos.Gamification.Data.Repository;

public class UserScoreRepository : IUserScoreRepository
{

    private UserScoreDbContext _context;
    private static SemaphoreSlim _semaphoreLike = new SemaphoreSlim(1, 1);

    public UserScoreRepository(UserScoreDbContext scoreContext)
    {
        _context = scoreContext;
        
    }


    public UserScore GetUserScore(string pUserId)
    {
        UserScore result = new();

        var score = _context.UserScores?.FirstOrDefault(u => u.Id == new Guid(pUserId));

        if (score != null)
        {
            result = score;
        }

        return result;
    }

    public bool SetUserScore(UserScore userScore)
    {

        UserScore? score;
        bool result = false;

        _semaphoreLike.Wait();

        try
        {

            score = _context.UserScores?
                    .FirstOrDefault(u => u.Id == userScore.Id);

            if (score != null)
            {

                score.Score += userScore.Score;
                score.KudosSent += userScore.KudosSent;
                score.KudosReceived += userScore.KudosReceived;
                score.LikesSent += userScore.LikesSent;
                score.LikesReceived += userScore.LikesReceived;
                score.MessagesReceived += userScore.MessagesReceived;
                score.MessagesSent += userScore.MessagesSent;

            }
            else
            {
                _context.UserScores?.Add(userScore);


            }

            result = _context.SaveChanges() > 0;

        }
        finally
        {
            _semaphoreLike.Release();
        }   
        

        return result;

    }

    public IEnumerable<UserScore> GetTopUserScores(int top) 
    {
    
        return _context.UserScores.Where(s=> s.Score > 0).OrderByDescending(s=> s.Score).Take(top);
    }

    public bool UpdateGroupScore(UserScore userScore)
    {
        UserScore? score;
        bool result = false;

        _semaphoreLike.Wait();
        
        try { 
            score = _context.UserScores?.FirstOrDefault(u => u.Id == userScore.Id);

            if (score != null)
            {

                score.GroupOne = userScore.GroupOne;
                score.GroupTwo = userScore.GroupTwo;
                score.GroupThree = userScore.GroupThree;
                score.GroupFour = userScore.GroupFour;
                score.GroupFive = userScore.GroupFive;
                score.GroupAll = userScore.GroupAll;

            }
            else
            {
                _context.UserScores?.Add(userScore);


            }

            result = _context.SaveChanges() > 0;
        }
        finally
        {
            _semaphoreLike.Release();
        }

        return result;
    }
}
