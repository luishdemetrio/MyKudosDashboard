
using Microsoft.AspNetCore.Components;
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

    public async Task<IEnumerable<UserViewModel>> GetUsersAsync(string name)
    {
        return await _dashboardService.GetUsers(name);              
    }

    public async Task<string> Send(KudosRequest kudos)
    {
     
        return await _dashboardService.SendKudos(kudos);
    }
}
