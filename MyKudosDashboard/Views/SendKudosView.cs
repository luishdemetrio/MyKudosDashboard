
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

    public IEnumerable<RecognitionViewModel> GetRecognitions()
    {
        return _dashboardService.GetRecognitions();
    }

    public IEnumerable<UserViewModel> GetUsers(string name)
    {
        return _dashboardService.GetUsers(name);              
    }

    public bool Send(KudosRequest kudos)
    {
     
        return _dashboardService.SendKudos(kudos);
    }
}
