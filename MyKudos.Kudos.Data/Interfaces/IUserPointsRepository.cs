using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Data.Interfaces;

public interface IUserPointsRepository
{

    UserPointScore GetUserScore(Guid pUserId);

    List<UserPoint> GetTopUserScores(int top);
}
