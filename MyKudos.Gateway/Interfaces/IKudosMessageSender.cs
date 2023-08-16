using MyKudos.Gateway.Domain.Models;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Gateway.Interfaces;

public interface IKudosMessageSender
{
    Task SendKudosAsync(int kudosId, Gateway.Domain.Models.KudosNotification kudos);

    Task SendLikeAsync(LikeGateway like, List<KudosReceiver> Recognized);

    Task SendUndoLikeAsync(LikeGateway like, List<KudosReceiver> Recognized);

    Task UpdateUserScore(Kudos.Domain.Models.UserPointScore userPointScore);
}
    