using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.EventHub.Enums;

namespace MyKudosDashboard.EventHub;

public interface IEventHubReceived<T>
{

    void Attach(string user, IObserverEventHub<T> observer);
    void Detach(string user);
}

public interface IEventHubLikeSent : IEventHubReceived<EventHubResponse<EventHubLikeOptions, LikeGateway>>
{
}


public interface IEventHubCommentDeleted : IEventHubReceived<EventHubResponse<EventHubCommentOptions, CommentsRequest>>
{
}

public interface IEventHubCommentSent : IEventHubReceived<EventHubResponse<EventHubCommentOptions, CommentsRequest>>
{
}

public interface IEventHubKudosSent : IEventHubReceived<KudosResponse>
{
}

public interface IEventHubKudosDeleted : IEventHubReceived<int>
{
}

public interface IEventHubKudosUpdated : IEventHubReceived<KudosMessage>
{
}

public interface IEventHubUndoLike : IEventHubReceived<EventHubResponse<EventHubLikeOptions, LikeGateway>>
{
}


public interface IEventHubUserPointsReceived : IEventHubReceived<UserPointScore>
{
}





