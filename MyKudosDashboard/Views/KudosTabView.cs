using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;
using Newtonsoft.Json;
using MyKudosDashboard.MessageSender;
using System.Threading;
using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.EventHub;
using MyKudosDashboard.EventHub.Enums;

namespace MyKudosDashboard.Views;

public class KudosTabView : IKudosTabView, 
                            IObserverEventHub<EventHubResponse<EventHubLikeOptions, LikeGateway>>, 
                            IObserverEventHub<KudosResponse>,
                            IObserverEventHub<EventHubResponse<EventHubCommentOptions, CommentsRequest>>
{ 
    private IKudosGateway _gatewayService;

   
    public IKudosTabView.UpdateLikesCallBack LikeCallback { get; set; }

    public IKudosTabView.UpdateLikesCallBack UndoLikeCallback { get; set; }

    public IKudosTabView.UpdateKudosCallBack KudosCallback { get; set; }

    public IKudosTabView.CommentsCallBack CommentsSentCallback { get; set; }
    public IKudosTabView.CommentsCallBack CommentsUpdatedCallback { get; set; }
    public IKudosTabView.CommentsCallBack CommentsDeletedCallback { get; set; }

   
    IEventHubReceived<EventHubResponse<EventHubLikeOptions, LikeGateway>> _eventHubUndoLikeSent;
    IEventHubReceived<EventHubResponse<EventHubLikeOptions, LikeGateway>> eventHubLikeSent;

    IEventHubReceived<EventHubResponse<EventHubCommentOptions, CommentsRequest>> _eventHubCommentSent;


    IEventHubReceived<KudosResponse> _eventHubKudosSent;


    public KudosTabView(IKudosGateway gatewayService, IConfiguration configuration,
                        ILogger<KudosTabView> logger)
    {
        _gatewayService = gatewayService;

      
        eventHubLikeSent = EventHubLikeSent.GetInstance(configuration);
        eventHubLikeSent.Attach(this);

        _eventHubUndoLikeSent = EventHubUndoLike.GetInstance(configuration);
        _eventHubUndoLikeSent.Attach(this);

        _eventHubCommentSent = EventHubCommentSent.GetInstance(configuration);
        _eventHubCommentSent.Attach(this);

        _eventHubKudosSent = EventHubKudosSent.GetInstance(configuration); ;
        _eventHubKudosSent.Attach(this);

    }


   

   
    public void UpdateMessageSent(CommentsRequest comments)
    {
        CommentsSentCallback?.Invoke(comments);
    }

    public void UpdateMessageDeleted(CommentsRequest comments)
    {
        CommentsUpdatedCallback?.Invoke(comments);
    }

    public void UpdateMessageUpdated(CommentsRequest comments)
    {
        CommentsDeletedCallback?.Invoke(comments);
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
}
