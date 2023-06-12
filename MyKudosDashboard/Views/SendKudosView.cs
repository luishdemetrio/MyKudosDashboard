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

    public async Task<IEnumerable<RecognitionViewModel>> GetRecognitionsAsync()
    {

        var recognitions = _recognitionGateway.GetRecognitionsAsync();

        var recognitionsGroup = _recognitionGateway.GetRecognitionGroups();

        await Task.WhenAll(recognitions, recognitionsGroup);

        var result = from g in recognitionsGroup.Result
                                      join r in recognitions.Result
                                        on g.RecognitionGroupId equals r.RecognitionGroupId
                                      select new RecognitionViewModel
                                      {
                                          Description = r.Description,
                                          DisplayOrder = r.DisplayOrder,
                                          Emoji = r.Emoji,
                                          GroupName = g.Description,
                                          RecognitionId = r.RecognitionId,
                                          Title = r.Title
                                      };




        return result;
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
