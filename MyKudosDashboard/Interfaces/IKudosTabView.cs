using MyKudos.Gateway.Domain.Models;

namespace MyKudosDashboard.Interfaces;

public interface IKudosTabView
{

    void RegisterObserver(string userId);
    void UnregisterObserver(string userId);

    public delegate void UpdateLikesCallBack(LikeGateway like);

    public UpdateLikesCallBack LikeCallback { get; set; }
    public UpdateLikesCallBack UndoLikeCallback { get; set; }


    public delegate void UpdateKudosCallBack(KudosResponse kudos);
    public UpdateKudosCallBack KudosCallback { get; set; }

    public delegate void CommentsCallBack(CommentsRequest like);
    public CommentsCallBack CommentsSentCallback { get; set; }
    public CommentsCallBack CommentsUpdatedCallback { get; set; }
    public CommentsCallBack CommentsDeletedCallback { get; set; }


    public delegate void KudosDeletedCallBack(int kudosId);
    public KudosDeletedCallBack KudosDeletedCallback { get; set; }


    public delegate void KudosUpdatedCallBack(KudosMessage message);
    public KudosUpdatedCallBack KudosMessageUpdatedCallBack { get; set; }

}
