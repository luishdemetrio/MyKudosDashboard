using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Views;

public class TopContributorsView : ITopContributorsView
{

    private IGamificationGateway _gamificationGateway;


    public TopContributorsView(IGamificationGateway gamificationGateway)
    {
        _gamificationGateway = gamificationGateway;
    }



    public async Task<IEnumerable<TopContributors>> GetTopContributors()
    {
        return await _gamificationGateway.GetTopContributors();
    }
}
