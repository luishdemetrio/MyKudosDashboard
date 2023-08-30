using SuperKudos.KudosCatalog.Domain.Models;

namespace SuperKudos.KudosCatalog.App.Interfaces;

public interface IUserPointsService
{

    List<UserPoint> GetTopUserScores(int top);

    UserPointScore GetUserScore(Guid pUserId);
}
