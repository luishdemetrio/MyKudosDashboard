using MyKudos.Gamification.Domain.Models;
using System.Threading.Tasks;

namespace MyKudos.Gamification.Receiver.Interfaces;

internal interface IGroupUserScoreService
{
    Task<bool> UpdateGroupScoreAsync(UserScore userScore);
}
