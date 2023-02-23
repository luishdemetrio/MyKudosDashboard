using MyKudos.Gateway.Models;

namespace MyKudos.Gateway.Interfaces;

public interface IKudosQueue
{
    Task SendAsync(KudosNotification kudos);
}