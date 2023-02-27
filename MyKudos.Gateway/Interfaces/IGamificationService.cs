using MyKudos.Gateway.Models;

namespace MyKudos.Gateway.Interfaces;

public interface IGamificationService
{

    Task<UserScore> GetUserScoreAsync(string pUserId);

    Task<IEnumerable<UserScore>> GetTopUserScoresAsync(int top);

}
