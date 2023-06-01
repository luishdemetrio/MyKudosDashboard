using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;
using Newtonsoft.Json;
using MyKudosDashboard.MessageSender;
using System.Threading;
using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.EventGrid;

namespace MyKudosDashboard.Views;

public class KudosTabView : IKudosTabView, IObserverKudos, IDisposable
{
    private IKudosGateway _gatewayService;

   
    public IKudosTabView.UpdateLikesCallBack LikeCallback { get; set; }

    public IKudosTabView.UpdateLikesCallBack UndoLikeCallback { get; set; }

    public IKudosTabView.UpdateKudosCallBack KudosCallback { get; set; }

    public IKudosTabView.CommentsCallBack CommentsSentCallback { get; set; }
    public IKudosTabView.CommentsCallBack CommentsUpdatedCallback { get; set; }
    public IKudosTabView.CommentsCallBack CommentsDeletedCallback { get; set; }



    private IEventGridKudosReceived _eventGridReceived;

    public KudosTabView(IKudosGateway gatewayService, IConfiguration configuration, ILogger<KudosTabView> logger, IEventGridKudosReceived eventGridReceived)
    {
        _gatewayService = gatewayService;

        _eventGridReceived = eventGridReceived;

        _eventGridReceived.Attach(this);
            
    }

    public void NotifyKudosCallBack(KudosResponse kudos)
    {
        KudosCallback?.Invoke(kudos);
    }

   
    public void UpdateLikeSent(LikeGateway like)
    {
        LikeCallback?.Invoke(like);
    }

    public void UpdateUndoLikeSent(LikeGateway like)
    {
        UndoLikeCallback?.Invoke(like);
    }

    public void UpdateKudosSent(KudosResponse kudos)
    {
        KudosCallback?.Invoke(kudos);
    }

    public void Dispose()
    {
        _eventGridReceived.Detach(this);
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
}
