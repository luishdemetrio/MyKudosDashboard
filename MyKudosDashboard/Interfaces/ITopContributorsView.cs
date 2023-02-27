using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface ITopContributorsView
{

    Task<IEnumerable<TopContributors>> GetTopContributors();
}
