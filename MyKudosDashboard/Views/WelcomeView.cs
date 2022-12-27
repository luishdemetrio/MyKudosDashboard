using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Views;

public class WelcomeView : IWelcomeView
{
    private IGraphService _graphHelper;
    private IGatewayService _gatewayService;

    public WelcomeView(IGraphService graphHelper, IGatewayService gatewayService)
    {
        _graphHelper = graphHelper;
        _gatewayService = gatewayService;
    }

    public IEnumerable<KudosResponse> GetKudos()
    {
        return _gatewayService.GetKudos();
    }

    public async Task<string> GetUserPhoto(string userid)
    {
        return await _graphHelper.GetUserPhoto(userid);
    }
}
