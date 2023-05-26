using MyKudos.Kudos.Domain.Models;
using System.Threading.Tasks;

namespace MyKudos.Gamification.Receiver.Interfaces;

public interface IGroupScoreRules
{
    Task<UserScore> UpdateGroupScoreAsync(UserScore pUserScore);
}
