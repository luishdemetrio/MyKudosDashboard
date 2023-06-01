using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Gateway.Interfaces;

public interface IAgentNotificationService
{

    Task<bool> SendNotificationAsync(KudosNotification kudos);
}