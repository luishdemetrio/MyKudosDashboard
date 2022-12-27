
using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Views;

public class SendKudosView : ISendKudosView
{
    private IGatewayService _dashboardService;
    private IGraphService _graphHelper;

    public SendKudosView(IGatewayService dashboardService, IGraphService graphHelper)
    {
        _dashboardService = dashboardService;
        _graphHelper = graphHelper;
    }

    public IEnumerable<RecognitionViewModel> GetRecognitions()
    {
        return _dashboardService.GetRecognitions();

    }

    public async Task<IEnumerable<UserViewModel>> GetUsersAsync(string name)
    {

        var graphUsers = await _graphHelper.GetUsers(name);

        if (graphUsers.value.Count()  == 0)
            return new List<UserViewModel>();

        var photos = await _graphHelper.GetUserPhotos(graphUsers);
                

        return (from graphUser in graphUsers.value
                 join photo in photos.responses
                     on graphUser.id equals photo.id
                 select new UserViewModel(graphUser.id, graphUser.displayName, "data:image/png;base64," + photo.body));
    }

    public bool Send(KudosRequest kudos)
    {
     
        return _dashboardService.SendKudos(kudos);
    }
}
