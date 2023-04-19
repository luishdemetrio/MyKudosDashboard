using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface IUserProfileScoreView
{

    Task<UserScore> GetUserScore(string userId);

    public void RegisterForLiveUpdates(string userId);

    public delegate void UpdateScoreCallBack(UserScore userScore);

    public UpdateScoreCallBack UserScoreCallback { get; set; }

}
