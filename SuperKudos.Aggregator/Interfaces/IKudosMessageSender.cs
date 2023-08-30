using SuperKudos.Aggregator.Domain.Models;
using SuperKudos.KudosCatalog.Domain.Models;

namespace SuperKudos.Aggregator.Interfaces;

public interface IKudosMessageSender
{
    Task SendKudosAsync(int kudosId, Aggregator.Domain.Models.KudosNotification kudos);

    Task SendLikeAsync(LikeGateway like, List<KudosReceiver> Recognized);

    Task SendUndoLikeAsync(LikeGateway like, List<KudosReceiver> Recognized);

    Task UpdateUserScore(SuperKudos.KudosCatalog.Domain.Models.UserPointScore userPointScore);
}
    