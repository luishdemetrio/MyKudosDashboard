using MyKudos.Gateway.Domain.Models;

namespace MyKudosDashboard.Interfaces;

public interface IGamificationGateway
{

    Task<UserPointScore> GetUserScoreAsync(string pUserId);

    Task<IEnumerable<TopContributors>> GetTopContributors(Guid? managerId);
}
