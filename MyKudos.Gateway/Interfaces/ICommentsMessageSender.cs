using MyKudos.Gateway.Domain.Models;

namespace MyKudos.Gateway.Interfaces;

public interface ICommentsMessageSender
{

    Task MessageSent(CommentsRequest comments);

    Task MessageDeleted(CommentsRequest comments);

    Task MessageUpdated(CommentsRequest comments);
}
