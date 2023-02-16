
using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Views;

public class SendKudosView : ISendKudosView
{
    private IGatewayService _dashboardService;
    

    public SendKudosView(IGatewayService dashboardService)
    {
        _dashboardService = dashboardService;        
    }

    public async Task<IEnumerable<RecognitionViewModel>> GetRecognitionsAsync()
    {
        return await _dashboardService.GetRecognitionsAsync();
    }

    public IEnumerable<UserViewModel> GetUsers(string name)
    {
        return _dashboardService.GetUsers(name);              
    }

    public string Send(KudosRequest kudos)
    {
     
        return _dashboardService.SendKudos(kudos);
    }
}
