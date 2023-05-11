using MyKudos.Gamification.Domain.Models;

namespace MyKudos.Gamification.Domain.Intefaces;

public interface IUserScoreRepository
{
    UserScore GetUserScore(string pUserId);

    bool SetUserScore(UserScore userScore);

    IEnumerable<UserScore> GetTopUserScores(int top);

    bool UpdateGroupScore(UserScore userScore);
}
