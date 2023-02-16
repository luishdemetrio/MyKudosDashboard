
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface IWelcomeView
{
    Task<string> GetUserPhoto(string userid);

    Task<IEnumerable<KudosResponse>> GetKudos();
}
