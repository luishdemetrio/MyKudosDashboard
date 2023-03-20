using MyKudos.Gamification.Domain.Models;
using System.Threading.Tasks;

namespace MyKudos.Gamification.Receiver.Interfaces;

public interface IScoreQueue
{
    Task NotifyProfileScoreUpdated(UserScore score);
}
