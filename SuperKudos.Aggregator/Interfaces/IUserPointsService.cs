using SuperKudos.KudosCatalog.Domain.Models;

namespace SuperKudos.Aggregator.Interfaces;

public interface IUserPointsService
{

    public Task<List<UserPoint>> GetTopUserScoresAsync(int top);

    public Task<UserPointScore> GetUserScoreAsync(Guid pUserId);

    



}
