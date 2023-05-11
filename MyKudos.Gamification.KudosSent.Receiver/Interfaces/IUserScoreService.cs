using MyKudos.Gamification.Domain.Models;
using System.Threading.Tasks;

namespace MyKudos.Gamification.Receiver.Interfaces;

internal interface IUserScoreService
{
    Task<UserScore> GetUserScoreAsync(string pUserId);

    Task<bool> SetUserScoreAsync(UserScore userScore);
}
