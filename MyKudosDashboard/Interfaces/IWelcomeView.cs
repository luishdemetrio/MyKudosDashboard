
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface IWelcomeView
{
    Task<string> GetUserPhoto(string userid);

    IEnumerable<KudosResponse> GetKudos();
}
