using MyKudos.Kudos.Domain.Models;
using System.Threading.Tasks;

namespace MyKudos.Gamification.Receiver.Interfaces;

public interface IScoreMessageSender
{
    Task NotifyProfileScoreUpdated(UserScore score);

    Task NotifyProfileScoreSamePersonUpdated(UserScore score);
}
