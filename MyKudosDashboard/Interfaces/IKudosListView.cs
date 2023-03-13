using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface IKudosListView
{

    Task<bool> SendLikeAsync(Like like);


    public delegate void UpdateLikeCallBack(Like like);

    public UpdateLikeCallBack LikeCallback { get; set; }


    public delegate void UpdateKudosCallBack(KudosResponse kudos);

    public UpdateKudosCallBack KudosCallback { get; set; }
}
