using SuperKudos.Aggregator.Domain.Models;

namespace SuperKudos.Aggregator.Interfaces;

public interface IAgentNotificationService
{

    Task<bool> SendNotificationAsync(KudosNotification kudos);
}