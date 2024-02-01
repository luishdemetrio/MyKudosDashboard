using MyKudos.Gateway.Domain.Models;

namespace MyKudosDashboard.Interfaces;

public interface IGamificationGateway
{

    Task<UserPointScore> GetUserScoreAsync(string pUserId, bool justMyTeam = false);

    Task<IEnumerable<TopContributors>> GetTopContributors(Guid? managerId);
}
