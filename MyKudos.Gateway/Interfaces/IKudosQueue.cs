using MyKudos.Gateway.Models;

namespace MyKudos.Gateway.Interfaces;

public interface IKudosQueue
{
    Task SendKudosAsync(string kudosId, KudosNotification kudos);

    Task SendLikeAsync(LikeGateway like, int sign );

    Task MessageSent(CommentsRequest comments);

    Task MessageDeleted(CommentsRequest comments);
}