using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface IUserProfileScoreView
{

    Task<UserScore> GetUserScore(string userId);

}
