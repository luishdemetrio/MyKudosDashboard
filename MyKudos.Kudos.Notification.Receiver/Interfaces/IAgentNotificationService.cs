

using MyKudos.Kudos.Domain.Models;
using System.Threading.Tasks;

namespace MyKudos.Kudos.Notification.Receiver.Interfaces;

public interface IAgentNotificationService
{

    Task<bool> SendNotificationAsync(KudosNotification kudos);
}