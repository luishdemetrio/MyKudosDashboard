using MyKudos.Gateway.Domain.Models;

namespace MyKudosDashboard.Interfaces;

public interface IGamificationGateway
{

    Task<UserPointScore> GetUserScoreAsync(string pUserId, int sentOnYear, bool justMyTeam = false );

    Task<IEnumerable<TopContributors>> GetTopContributors(int sentOnYear, Guid? managerId);
}
