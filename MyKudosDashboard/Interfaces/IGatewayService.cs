using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface IGatewayService
{
    IEnumerable<RecognitionViewModel> GetRecognitions();

    string SendKudos(KudosRequest kudos);

    IEnumerable<KudosResponse> GetKudos();

    IEnumerable<UserViewModel> GetUsers(string name);

    Task<string> GetUserPhoto(string userid);

    bool SendLike(Like like);
}
