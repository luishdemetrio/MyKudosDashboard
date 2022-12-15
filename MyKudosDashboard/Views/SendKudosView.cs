using MyKudosDashboard.Helper;
using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Views;

public class SendKudosView : ISendKudosView
{
    private IDashboardService _dashboardService;

    public SendKudosView(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public IEnumerable<RecognitionViewModel> GetRecognitions()
    {
        return _dashboardService.GetRecognitions();

    }

    public async Task<IEnumerable<UserViewModel>> GetUsersAsync(string name)
    {

        var graphUsers = await GraphHelper.GetUsers(name);

        if (graphUsers.value.Count()  == 0)
            return new List<UserViewModel>();

        var photos = await GraphHelper.GetUserPhotos(graphUsers);
                

        return (from graphUser in graphUsers.value
                 join photo in photos.responses
                     on graphUser.id equals photo.id
                 select new UserViewModel(graphUser.id, graphUser.displayName, "data:image/png;base64," + photo.body));
    }

    public bool Send(KudosViewModel kudos)
    {
        return _dashboardService.SendKudos(kudos);
    }
}
