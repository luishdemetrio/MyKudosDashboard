using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Interfaces.Aggregator;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Views;

public class SendKudosView : ISendKudosView
{
    private IKudosGateway _kudosGateway;

    private IRecognitionGateway _recognitionGateway;

    private IRecognitionGroupAggregator _recognitionGroupAggregator;

    private IUserGateway _userGateway;

    
    public SendKudosView(IKudosGateway dashboardService, IRecognitionGateway recognitionGateway, 
                         IRecognitionGroupAggregator recognitionGroup,
                         IUserGateway userGateway)
    {
        _kudosGateway = dashboardService;
        _recognitionGateway = recognitionGateway;
        _userGateway = userGateway;

        _recognitionGroupAggregator = recognitionGroup;


    }

    public async Task<IEnumerable<RecognitionViewModel>> GetRecognitionsAsync()
    {

        var recognitions = _recognitionGateway.GetRecognitionsAsync();

        var recognitionsGroup = _recognitionGroupAggregator.GetRecognitionGroups();

        await Task.WhenAll(recognitions, recognitionsGroup);

        var result = from g in recognitionsGroup.Result
                                      join r in recognitions.Result
                                        on g.RecognitionGroupId equals r.RecognitionGroupId
                                      select new RecognitionViewModel
                                      {
                                          Description = r.Description,
                                          DisplayOrder = r.DisplayOrder,
                                          Emoji = g.Emoji,
                                          GroupName = g.Description,
                                          RecognitionId = r.RecognitionId,
                                          Title = r.Title
                                      };




        return result;
    }

    public async Task<IEnumerable<Person>> GetUsersAsync(string name)
    {
        return await _userGateway.GetUsers(name);              
    }

    public async Task<string> Send(SendKudosRequest kudos)
    {
     
        return await _kudosGateway.SendKudos(kudos);
    }

}
