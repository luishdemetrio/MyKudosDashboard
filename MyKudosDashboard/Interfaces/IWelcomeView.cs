
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface IWelcomeView
{
    Task<string> GetUserPhoto(string userId);

    public delegate void UpdateScoreCallBack(UserScore userScore);

    public UpdateScoreCallBack ScoreCallback { get; set; }

    public void RegisterForUserScoreUpdate(string userId);  


}
