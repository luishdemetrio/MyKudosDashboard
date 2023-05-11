using MyKudos.Gamification.Domain.Models;


namespace MyKudos.Gamification.App.Interfaces;

public interface IUserScoreService
{
    UserScore GetUserScore(string pUserId);

    bool SetUserScore(UserScore userScore);

    IEnumerable<UserScore> GetTopUserScores(int top);

    bool UpdateGroupScore(UserScore userScore);
}
