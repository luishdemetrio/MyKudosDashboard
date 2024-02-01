using MyKudos.Gateway.Domain.Models;

namespace MyKudosDashboard.Interfaces;

public interface IUserProfileScoreView
{

    void RegisterObserver(string userId);
    void UnregisterObserver(string userId);

    Task<UserPointScore> GetUserScore(string userId, bool justMyTeam);

    
    public delegate void UpdateScoreCallBack(UserPointScore userScore);

    public UpdateScoreCallBack UserScoreCallback { get; set; }

    

}
