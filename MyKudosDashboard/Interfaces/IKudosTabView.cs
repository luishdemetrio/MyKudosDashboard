using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface IKudosTabView
{

    //Task<IEnumerable<KudosResponse>> GetKudos(int pageNumber);

    public void RegisterForLiveUpdates(string userId);

    public delegate void UpdateLikesCallBack(LikeGateway like);

    public UpdateLikesCallBack LikeCallback { get; set; }
    public UpdateLikesCallBack UndoLikeCallback { get; set; }


    public delegate void UpdateKudosCallBack(KudosResponse kudos);

    public UpdateKudosCallBack KudosCallback { get; set; }


    public delegate void CommentsCallBack(CommentsRequest like);

    public CommentsCallBack CommentsSentCallback { get; set; }
    public CommentsCallBack CommentsUpdatedCallback { get; set; }
    public CommentsCallBack CommentsDeletedCallback { get; set; }
}
