using Microsoft.EntityFrameworkCore;
using MyKudos.Gamification.Data.Context;
using MyKudos.Gamification.Domain.Intefaces;
using MyKudos.Gamification.Domain.Models;

namespace MyKudos.Gamification.Data.Repository;

public class UserScoreRepository : IUserScoreRepository
{

    private UserScoreDbContext _context;

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
        var score = _context.UserScores?.FirstOrDefault(u => u.UserId == userScore.UserId);

        if (score != null)
        {
            
            score.Score += userScore.Score;
            score.KudosSent += userScore.KudosSent;
            score.KudosReceived += userScore.KudosReceived;
            score.LikesSent += userScore.LikesSent;
            score.LikesReceived += userScore.LikesReceived;
            
          
        }
        else
        {
            _context.UserScores?.Add(userScore);
            

        }

        return _context.SaveChanges() > 0;
    }
}
