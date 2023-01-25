using MyKudos.Gateway.Models;

namespace MyKudos.Gateway.Interfaces;

public interface IAgentNotificationService
{

    bool SendNotification(KudosNotification kudos);
}