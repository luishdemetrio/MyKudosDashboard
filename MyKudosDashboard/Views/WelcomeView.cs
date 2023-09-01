using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.Interfaces;

namespace MyKudosDashboard.Views;

public class WelcomeView : IWelcomeView
{

    private IUserGateway _userGateway;
    
    private static string _kudosSentDashboard = string.Empty;

    private static string _likeSentDashboard = string.Empty;
    private static string _likeUndoDashboard = string.Empty;

    private static string _commentSentDashboard = string.Empty;
    private static string _commentDeletedDashboard = string.Empty;

    public WelcomeView(IUserGateway userGateway)
    {

        _userGateway = userGateway;
    }

    public async Task<UserProfile> GetUserInfo(string userId)
    {

        return await _userGateway.GetUserInfo(userId);
    }

    

}
