using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Views;

public class UserProfileScoreView : IUserProfileScoreView
{

    private IGatewayService _dashboardService;


    public UserProfileScoreView(IGatewayService dashboardService)
    {
        _dashboardService = dashboardService;
    }


    public async Task<UserScore> GetUserScore(string userId)
    {
        return await _dashboardService.GetUserScoreAsync(userId);
    }
}
