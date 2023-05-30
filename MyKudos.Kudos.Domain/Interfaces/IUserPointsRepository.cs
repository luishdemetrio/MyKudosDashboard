using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Domain.Interfaces;

public interface IUserPointsRepository
{

    UserPointScore GetUserScore(string pUserId);

    List<UserPoint> GetTopUserScores(int top);
}
