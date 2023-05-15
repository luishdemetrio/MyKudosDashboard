using MyKudos.Gamification.Domain.Models;
using System.Threading.Tasks;

namespace MyKudos.Gamification.Receiver.Interfaces;

public interface IUserScoreService
{
    Task<UserScore> GetUserScoreAsync(string pUserId);

    Task<bool> SetUserScoreAsync(UserScore userScore);
}
