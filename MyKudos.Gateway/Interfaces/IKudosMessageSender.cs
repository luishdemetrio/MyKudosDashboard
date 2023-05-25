using MyKudos.Gateway.Domain.Models;

namespace MyKudos.Gateway.Interfaces;

public interface IKudosMessageSender
{
    Task SendKudosAsync(int kudosId, KudosNotification kudos);

    Task SendLikeAsync(LikeGateway like);

    Task SendUndoLikeAsync(LikeGateway like);

}