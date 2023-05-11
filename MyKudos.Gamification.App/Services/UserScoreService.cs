using MyKudos.Gamification.App.Interfaces;
using MyKudos.Gamification.Domain.Intefaces;
using MyKudos.Gamification.Domain.Models;


namespace MyKudos.Gamification.App.Services;

public class UserScoreService : IUserScoreService
{

    private readonly IUserScoreRepository _userScoreRepository;

    public UserScoreService(IUserScoreRepository userScoreRepository)
    {
        _userScoreRepository = userScoreRepository;
    }


    public UserScore GetUserScore(string pUserId)
    {
       return _userScoreRepository.GetUserScore(pUserId);
    }

    public bool SetUserScore(UserScore userScore)
    {
        return _userScoreRepository.SetUserScore(userScore);
    }

    public IEnumerable<UserScore> GetTopUserScores(int top)
    {
        return _userScoreRepository.GetTopUserScores(top);
    }

    public bool UpdateGroupScore(UserScore userScore)
    {
        return _userScoreRepository.UpdateGroupScore(userScore);
    }
}
