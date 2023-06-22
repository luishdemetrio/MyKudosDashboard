using MyKudosDashboard.Interfaces;
using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.EventHub;
using MyKudosDashboard.EventHub.Enums;
using Microsoft.Graph.ExternalConnectors;
using Microsoft.Extensions.Logging.ApplicationInsights;
using System.Diagnostics.Eventing.Reader;

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


    IEventHubUndoLike _eventHubUndoLikeSent;
    IEventHubLikeSent _eventHubLikeSent;

    IEventHubCommentSent _eventHubCommentSent;
    IEventHubCommentDeleted _eventHubCommentDeleted;


    IEventHubKudosSent _eventHubKudosSent;

    private IConfiguration _configuration;
    private string _userId;

    private ILogger<KudosTabView> _logger;


    public KudosTabView(IConfiguration configuration,
                        ILogger<KudosTabView> logger,
                        IEventHubLikeSent eventHubLikeSent,
                        IEventHubUndoLike eventHubUndoLike,
                        IEventHubCommentSent eventHubCommentsSent,
                        IEventHubCommentDeleted eventHubCommentDeleted,
                        IEventHubKudosSent eventHubKudosSent)
    {


        _configuration = configuration;
        _logger = logger;

        _eventHubLikeSent = eventHubLikeSent;
        _eventHubUndoLikeSent = eventHubUndoLike;

        _eventHubCommentSent = eventHubCommentsSent;

        _eventHubCommentDeleted = eventHubCommentDeleted;

        _eventHubKudosSent = eventHubKudosSent;
    }

    public void RegisterObserver(string userId)
    {
        //it is used on Dispose
        _userId = userId;
        
        _eventHubLikeSent.Attach(userId, this);

        _eventHubUndoLikeSent.Attach(userId, this);

        _eventHubCommentSent.Attach(userId, this);

        _eventHubCommentDeleted.Attach(userId, this);

        _eventHubKudosSent.Attach(userId, this);
        
    }
    

    public void NotifyUpdate(KudosResponse score)
    {

        KudosCallback?.Invoke(score);

       
    }

    public void NotifyUpdate(EventHubResponse<EventHubLikeOptions, LikeGateway> score)
    {
        _logger.LogInformation($"KudosTab Like/Undo EventHubCommentOptions: \n {score.Kind}");
        _logger.LogInformation($"KudosTab Like/Undo received: \n {System.Text.Json.JsonSerializer.Serialize<LikeGateway>(score.Event)}");

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

        _logger.LogInformation($"KudosTab EventHubCommentOptions: \n {score.Kind}");
        _logger.LogInformation($"KudosTab received: \n {System.Text.Json.JsonSerializer.Serialize<CommentsRequest>(score.Event)}");

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
        if (_eventHubLikeSent != null)
            _eventHubLikeSent.Detach(userId);

        if (_eventHubUndoLikeSent != null)
            _eventHubUndoLikeSent.Detach(userId);

        if (_eventHubCommentSent != null)
            _eventHubCommentSent.Detach(userId);

        if (_eventHubCommentDeleted != null)
            _eventHubCommentDeleted.Detach(userId);

        if (_eventHubKudosSent != null)
            _eventHubKudosSent.Detach(userId);
    }

    public void Dispose()
    {
        UnregisterObserver(_userId);
    }
}
