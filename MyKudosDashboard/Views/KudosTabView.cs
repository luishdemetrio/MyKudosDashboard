using MyKudosDashboard.Interfaces;
using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.EventHub;
using MyKudosDashboard.EventHub.Enums;
using Microsoft.Graph.ExternalConnectors;

namespace MyKudosDashboard.Views;

public class KudosTabView : IKudosTabView, 
                            IObserverEventHub<EventHubResponse<EventHubLikeOptions, LikeGateway>>, 
                            IObserverEventHub<KudosResponse>,
                            IObserverEventHub<EventHubResponse<EventHubCommentOptions, CommentsRequest>>, IDisposable
{ 
  
   
    public IKudosTabView.UpdateLikesCallBack LikeCallback { get; set; }

    public IKudosTabView.UpdateLikesCallBack UndoLikeCallback { get; set; }

    public IKudosTabView.UpdateKudosCallBack KudosCallback { get; set; }

    public IKudosTabView.CommentsCallBack CommentsSentCallback { get; set; }
    public IKudosTabView.CommentsCallBack CommentsUpdatedCallback { get; set; }
    public IKudosTabView.CommentsCallBack CommentsDeletedCallback { get; set; }

   
    IEventHubReceived<EventHubResponse<EventHubLikeOptions, LikeGateway>> _eventHubUndoLikeSent;
    IEventHubReceived<EventHubResponse<EventHubLikeOptions, LikeGateway>> eventHubLikeSent;

    IEventHubReceived<EventHubResponse<EventHubCommentOptions, CommentsRequest>> _eventHubCommentSent;
    IEventHubReceived<EventHubResponse<EventHubCommentOptions, CommentsRequest>> _eventHubCommentDeleted;


    IEventHubReceived<KudosResponse> _eventHubKudosSent;

    private IConfiguration _configuration;
    private string _userId;


    public KudosTabView(IConfiguration configuration,
                        ILogger<KudosTabView> logger)
    {


        _configuration = configuration;

    }

    public void RegisterObserver(string userId)
    {
        //it is used on Dispose
        _userId = userId;

        eventHubLikeSent = EventHubLikeSent.GetInstance(_configuration);
        eventHubLikeSent.Attach(userId, this);

        _eventHubUndoLikeSent = EventHubUndoLike.GetInstance(_configuration);
        _eventHubUndoLikeSent.Attach(userId, this);

        _eventHubCommentSent = EventHubCommentSent.GetInstance(_configuration);
        _eventHubCommentSent.Attach(userId, this);

        _eventHubCommentDeleted = EventHubCommentDeleted.GetInstance(_configuration);
        _eventHubCommentDeleted.Attach(userId, this);

        _eventHubKudosSent = EventHubKudosSent.GetInstance(_configuration); ;
        _eventHubKudosSent.Attach(userId, this);
    }

    public void NotifyUpdate(KudosResponse score)
    {

        KudosCallback?.Invoke(score);

       
    }

    public void NotifyUpdate(EventHubResponse<EventHubLikeOptions, LikeGateway> score)
    {
        switch (score.Kind)
        {
            case EventHubLikeOptions.LikeSent:
                LikeCallback?.Invoke(score.Event);
                break;
            case EventHubLikeOptions.UndoLike:
                UndoLikeCallback?.Invoke(score.Event);
                break;


        }
    }

    public void NotifyUpdate(EventHubResponse<EventHubCommentOptions, CommentsRequest> score)
    {
        switch (score.Kind)
        {

            
            case EventHubCommentOptions.CommentSent:
                CommentsSentCallback?.Invoke(score.Event);
                break;
            case EventHubCommentOptions.CommentUpdated:
                CommentsUpdatedCallback?.Invoke(score.Event);
                break;
            case EventHubCommentOptions.CommentDeleted:
                CommentsDeletedCallback?.Invoke(score.Event);
                break;
            default:
                break;
        }
    }

    public void UnregisterObserver(string userId)
    {
        eventHubLikeSent.Detach(userId);

        _eventHubUndoLikeSent.Detach(userId);

        _eventHubCommentSent.Detach(userId);

        _eventHubCommentDeleted.Detach(userId);

        _eventHubKudosSent.Detach(userId);
    }

    public void Dispose()
    {
        UnregisterObserver(_userId);
    }
}
