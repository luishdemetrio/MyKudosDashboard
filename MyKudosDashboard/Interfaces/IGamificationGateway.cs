using SuperKudos.Aggregator.Domain.Models;

namespace MyKudosDashboard.Interfaces;

public interface IGamificationGateway
{

    Task<UserPointScore> GetUserScoreAsync(string pUserId);

    Task<IEnumerable<TopContributors>> GetTopContributors();
}
