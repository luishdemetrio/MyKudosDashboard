using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Gateway.Interfaces;

public interface IUserPointsService
{

    public Task<List<UserPoint>> GetTopUserScoresAsync(int top, Guid? managerId, int? sentOnYear = null);

    public Task<UserPointScore> GetUserScoreAsync(Guid pUserId, bool justMyTeam = false, int? sentOnYear = null);

}
