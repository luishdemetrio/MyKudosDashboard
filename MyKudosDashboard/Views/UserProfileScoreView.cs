using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Views;

public class UserProfileScoreView : IUserProfileScoreView
{

    private IGamificationGateway _gamificationGateway;


    public UserProfileScoreView(IGamificationGateway gamificationGateway)
    {
        _gamificationGateway = gamificationGateway;
    }


    public async Task<UserScore> GetUserScore(string userId)
    {
        return await _gamificationGateway.GetUserScoreAsync(userId);
    }
}
