using SuperKudos.Aggregator.Domain.Models;

namespace SuperKudos.Aggregator.Interfaces;

public interface IGamificationService
{

    Task<UserScore> GetUserScoreAsync(string pUserId);

    Task<IEnumerable<UserScore>> GetTopUserScoresAsync(int top);

}
