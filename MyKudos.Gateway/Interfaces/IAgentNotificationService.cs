
namespace MyKudos.Gateway.Interfaces;

public interface IAgentNotificationService
{

    Task<bool> SendNotificationAsync(Gateway.Domain.Models.KudosNotification kudos);
}