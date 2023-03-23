using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface ICommentsView
{

    Task<bool> SendLikeAsync(Like like);


    public delegate void UpdateLikeCallBack(Like like);

    public UpdateLikeCallBack LikeCallback { get; set; }


    public delegate void UpdateCommentsCallBack(KudosResponse kudos);

    public UpdateCommentsCallBack CommentsCallback { get; set; }


    Task<string> SendComments(CommentsRequest comment);

    Task<IEnumerable<CommentsResponse>> GetComments(string kudosId);
}
