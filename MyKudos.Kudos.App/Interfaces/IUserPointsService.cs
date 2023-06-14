using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.App.Interfaces;

public interface IUserPointsService
{

    List<UserPoint> GetTopUserScores(int top);

    UserPointScore GetUserScore(Guid pUserId);
}
