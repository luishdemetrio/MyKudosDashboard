using MyKudosDashboard.Helper;
using MyKudosDashboard.Interfaces;

namespace MyKudosDashboard.Views;

public class WelcomeView : IWelcomeView
{
    public async Task<string> GetUserPhoto(string userid)
    {
        return await GraphHelper.GetUserPhoto(userid);
    }
}
