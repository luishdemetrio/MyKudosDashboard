using MyKudos.Gateway.Domain.Models;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Gateway.Interfaces;

public interface ICommentsMessageSender
{

    Task MessageSent(CommentsRequest comments, List<KudosReceiver> recognized);

    Task MessageDeleted(CommentsRequest comments, List<KudosReceiver> recognized);

    Task MessageUpdated(CommentsRequest comments);
}
