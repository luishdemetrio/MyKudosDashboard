using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Views;

public class KudosTabView : IKudosTabView
{
    private IGatewayService _gatewayService;

    public KudosTabView(IGatewayService gatewayService)
    {
        _gatewayService = gatewayService;
    }

    public async Task<IEnumerable<KudosResponse>> GetKudos()
    {
        return await _gatewayService.GetKudos();
    }
}
