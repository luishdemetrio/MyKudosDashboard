using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface IKudosTabView
{

    Task<IEnumerable<KudosResponse>> GetKudos();

    public void RegisterForLiveUpdates(string userId);

    public delegate void UpdateLikesCallBack(Like like);

    public UpdateLikesCallBack LikeCallback { get; set; }
    public UpdateLikesCallBack UndoLikeCallback { get; set; }


    public delegate void UpdateKudosCallBack(KudosResponse kudos);

    public UpdateKudosCallBack KudosCallback { get; set; }
}
