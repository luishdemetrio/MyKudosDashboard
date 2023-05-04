using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface IKudosTabView
{

    Task<IEnumerable<KudosResponse>> GetKudos(int pageNumber);

    public void RegisterForLiveUpdates(string userId);

    public delegate Task UpdateLikesCallBack(Like like);

    public UpdateLikesCallBack LikeCallback { get; set; }
    public UpdateLikesCallBack UndoLikeCallback { get; set; }


    public delegate Task UpdateKudosCallBack(KudosResponse kudos);

    public UpdateKudosCallBack KudosCallback { get; set; }


    public delegate Task CommentsCallBack(CommentsRequest like);

    public CommentsCallBack CommentsSentCallback { get; set; }
    public CommentsCallBack CommentsUpdatedCallback { get; set; }
    public CommentsCallBack CommentsDeletedCallback { get; set; }
}
