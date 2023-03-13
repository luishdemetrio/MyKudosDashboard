using MyKudosDashboard.Interfaces;

namespace MyKudosDashboard.Views;

public class WelcomeView : IWelcomeView
{
    
    private IGatewayService _gatewayService;

    public WelcomeView(IGatewayService gatewayService)
    {   
        _gatewayService = gatewayService;
    }

    public async Task<string> GetUserPhoto(string userid)
    {

        return await _gatewayService.GetUserPhoto(userid);
    }
}
