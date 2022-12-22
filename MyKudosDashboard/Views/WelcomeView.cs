using MyKudosDashboard.Interfaces;

namespace MyKudosDashboard.Views;

public class WelcomeView : IWelcomeView
{
    private IGraphService _graphHelper;

    public WelcomeView(IGraphService graphHelper)
    {
        _graphHelper = graphHelper;
    }
    public async Task<string> GetUserPhoto(string userid)
    {
        return await _graphHelper.GetUserPhoto(userid);
    }
}
