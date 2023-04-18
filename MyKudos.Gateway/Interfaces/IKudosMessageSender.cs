using MyKudos.Gateway.Models;

namespace MyKudos.Gateway.Interfaces;

public interface IKudosMessageSender
{
    Task SendKudosAsync(string kudosId, KudosNotification kudos);

    Task SendLikeAsync(LikeGateway like);

    Task SendUndoLikeAsync(LikeGateway like);

}