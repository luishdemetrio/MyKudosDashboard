using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Views;

public class WelcomeView : IWelcomeView
{
    
    private IGatewayService _gatewayService;

    public WelcomeView(IGatewayService gatewayService)
    {   
        _gatewayService = gatewayService;
    }

    public IEnumerable<KudosResponse> GetKudos()
    {
        return _gatewayService.GetKudos();
    }

    public async Task<string> GetUserPhoto(string userid)
    {

        return await _gatewayService.GetUserPhoto(userid);
    }
}
