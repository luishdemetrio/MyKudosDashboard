using MyKudos.Kudos.Domain.Models;
using System.Threading.Tasks;

namespace MyKudos.Gamification.Receiver.Interfaces;

public interface IGroupUserScoreService
{
    Task<bool> UpdateGroupScoreAsync(UserScore userScore);
}
