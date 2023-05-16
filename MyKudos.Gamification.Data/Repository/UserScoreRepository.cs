using Microsoft.EntityFrameworkCore;
using MyKudos.Gamification.Data.Context;
using MyKudos.Gamification.Domain.Intefaces;
using MyKudos.Gamification.Domain.Models;

namespace MyKudos.Gamification.Data.Repository;

public class UserScoreRepository : IUserScoreRepository
{

    private UserScoreDbContext _context;
    private readonly object _lock = new object();

    public UserScoreRepository(UserScoreDbContext scoreContext)
    {
        _context = scoreContext;
        
    }


    public UserScore GetUserScore(string pUserId)
    {
        return _context.UserScores?.FirstOrDefault(u => u.UserId == pUserId);
    }

    public bool SetUserScore(UserScore userScore)
    {

        UserScore score;

        lock (_lock)
        {
            score = _context.UserScores?.FirstOrDefault(u => u.UserId == userScore.UserId);

            if (score != null)
            {

                score.Score += userScore.Score;
                score.KudosSent += userScore.KudosSent;
                score.KudosReceived += userScore.KudosReceived;
                score.LikesSent += userScore.LikesSent;
                score.LikesReceived += userScore.LikesReceived;
                score.MessagesReceived += userScore.MessagesReceived;
                score.MessagesSent += userScore.MessagesSent;

                if (score.KudosSent < 0)
                    score.KudosSent = 0;

                if (score.KudosReceived < 0)
                    score.KudosReceived = 0;

                if (score.LikesSent < 0)
                    score.LikesSent = 0;

                if (score.LikesReceived < 0)
                    score.LikesReceived = 0;


                if (score.MessagesReceived < 0)
                    score.MessagesReceived = 0;

                if (score.MessagesSent < 0)
                    score.MessagesSent = 0;

            }
            else
            {
                _context.UserScores?.Add(userScore);


            }
        }
        return _context.SaveChanges() > 0;
    }

    public IEnumerable<UserScore> GetTopUserScores(int top) 
    {
    
        return _context.UserScores.Where(s=> s.Score > 0).OrderByDescending(s=> s.Score).Take(top);
    }

    public bool UpdateGroupScore(UserScore userScore)
    {
        UserScore score;

        lock (_lock)
        {
            score = _context.UserScores?.FirstOrDefault(u => u.UserId == userScore.UserId);

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
        }
        return _context.SaveChanges() > 0;
    }
}
