using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface IGatewayService
{
    Task<IEnumerable<RecognitionViewModel>> GetRecognitionsAsync();

    Task<string> SendKudos(KudosRequest kudos);

    Task<IEnumerable<KudosResponse>> GetKudos();

    Task<IEnumerable<UserViewModel>> GetUsers(string name);

    Task<string> GetUserPhoto(string userid);

    Task<bool> SendLike(Like like);
}
