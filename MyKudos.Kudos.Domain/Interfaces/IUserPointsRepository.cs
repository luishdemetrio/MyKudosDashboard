using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Domain.Interfaces;

public interface IUserPointsRepository
{

    UserPointScore GetUserScore(Guid pUserId, bool justMyTeam = false, int? sentOnYear = null);

    List<UserPoint> GetTopUserScores(int top, Guid? managerId, int? sentOnYear = null);

   
}
