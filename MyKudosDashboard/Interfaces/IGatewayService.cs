using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface IGatewayService
{
    Task<IEnumerable<RecognitionViewModel>> GetRecognitionsAsync();

    string SendKudos(KudosRequest kudos);

    IEnumerable<KudosResponse> GetKudos();

    IEnumerable<UserViewModel> GetUsers(string name);

    Task<string> GetUserPhoto(string userid);

    bool SendLike(Like like);
}
