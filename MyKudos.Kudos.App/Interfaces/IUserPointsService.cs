using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.App.Interfaces;

public interface IUserPointsService
{

    List<UserPoint> GetTopUserScores(int top, int visualizeJustMyTeam);

    UserPointScore GetUserScore(Guid pUserId);
}
