
namespace MyKudosDashboard.Interfaces;

public interface IWelcomeView
{
    Task<string> GetUserPhoto(string userid);

}
