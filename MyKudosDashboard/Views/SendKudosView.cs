using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Views;

public class SendKudosView : ISendKudosView
{
    private IKudosGateway _kudosGateway;

    private IRecognitionGateway _recognitionGateway;

    private IUserGateway _userGateway;

    public SendKudosView(IKudosGateway dashboardService, IRecognitionGateway recognitionGateway, IUserGateway userGateway)
    {
        _kudosGateway = dashboardService;
        _recognitionGateway = recognitionGateway;
        _userGateway = userGateway;
    }

    public async Task<IEnumerable<Recognition>> GetRecognitionsAsync()
    {
        return await _recognitionGateway.GetRecognitionsAsync();
    }

    public async Task<IEnumerable<UserViewModel>> GetUsersAsync(string name)
    {
        return await _userGateway.GetUsers(name);              
    }

    public async Task<string> Send(KudosRequest kudos)
    {
     
        return await _kudosGateway.SendKudos(kudos);
    }
}
