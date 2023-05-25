
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.App.Interfaces;

public interface IUserScoreService
{
    UserScore GetUserScore(string pUserId);

    UserScore? SetUserScore(UserScore userScore);

    IEnumerable<UserScore> GetTopUserScores(int top);

    bool UpdateGroupScore(UserScore userScore);
}

