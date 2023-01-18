using MyKudos.Agent.Models;

namespace MyKudos.Agent.Interfaces;

public interface IAgentNotification
{

    Task<bool> SendNotification(Kudos kudos);

}
