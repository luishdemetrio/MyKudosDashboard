using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Views;

public class TopContributorsView : ITopContributorsView
{

    private IGatewayService _dashboardService;


    public TopContributorsView(IGatewayService dashboardService)
    {
        _dashboardService = dashboardService;
    }



    public async Task<IEnumerable<TopContributors>> GetTopContributors()
    {
        return await _dashboardService.GetTopContributors();
    }
}
