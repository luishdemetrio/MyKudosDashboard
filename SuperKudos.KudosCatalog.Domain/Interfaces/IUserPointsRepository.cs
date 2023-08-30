using SuperKudos.KudosCatalog.Domain.Models;

namespace SuperKudos.KudosCatalog.Domain.Interfaces;

public interface IUserPointsRepository
{

    UserPointScore GetUserScore(Guid pUserId);

    List<UserPoint> GetTopUserScores(int top);
}
