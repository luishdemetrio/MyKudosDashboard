using MyKudos.Gateway.Domain.Models;

namespace MyKudosDashboard.Interfaces;

public interface IUserProfileScoreView
{

    Task<UserPointScore> GetUserScore(string userId);


    public void RegisterForLiveUpdates(string userId);

    public delegate void UpdateScoreCallBack(UserPointScore userScore);

    public UpdateScoreCallBack UserScoreCallback { get; set; }

}
