using MyKudosDashboard.Helper;
using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Views;

public class SendKudosView : ISendKudosView
{
    public IEnumerable<RecognitionViewModel> GetRecognitions()
    {
        List<RecognitionViewModel> recognitions = new();

        recognitions.Add(new RecognitionViewModel("🏆", "Awesome", false));
        recognitions.Add(new RecognitionViewModel("✨", "Thank you", false));
        recognitions.Add(new RecognitionViewModel("🎉", "Congratulations", false));
        recognitions.Add(new RecognitionViewModel("🏅", "Achiever", false));
        recognitions.Add(new RecognitionViewModel("💡", "Problem Solver", false));
        recognitions.Add(new RecognitionViewModel("🦁", "Courage", false));
        recognitions.Add(new RecognitionViewModel("🏀", "Team Player", false));

        return recognitions;
    }

    public async Task<IEnumerable<UserViewModel>> GetUsersAsync(string name)
    {

        var graphUsers = await GraphHelper.GetUsers(name);

        var photos = await GraphHelper.GetUserPhotos(graphUsers);

        return (from graphUser in graphUsers.value
                 join photo in photos.responses
                     on graphUser.id equals photo.id
                 select new UserViewModel(graphUser.id, graphUser.displayName, "data:image/png;base64," + photo.body));
    }

    public bool Send()
    {
        throw new NotImplementedException();
    }
}
