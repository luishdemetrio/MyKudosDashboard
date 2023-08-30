using SuperKudos.Aggregator.Domain.Models;
using SuperKudos.KudosCatalog.Domain.Models;

namespace SuperKudos.Aggregator.Interfaces;

public interface ICommentsMessageSender
{

    Task MessageSent(CommentsRequest comments, List<KudosReceiver> recognized);

    Task MessageDeleted(CommentsRequest comments, List<KudosReceiver> recognized);

    Task MessageUpdated(CommentsRequest comments);
}
