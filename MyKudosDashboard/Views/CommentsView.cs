using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Views;

public class CommentsView : ICommentsView
{
    public ICommentsView.UpdateLikeCallBack LikeCallback { get ; set  ; }
    public ICommentsView.UpdateCommentsCallBack CommentsCallback { get ; set ; }


    private IGatewayService _dashboardService;


    public CommentsView(IGatewayService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public Task<IEnumerable<CommentsResponse>> GetComments(string kudosId)
    {
        return _dashboardService.GetComments(kudosId);
    }

    public Task<string> SendComments(CommentsRequest comment)
    {
        return _dashboardService.SendCommentsAsync(comment); 
    }

    public Task<bool> SendLikeAsync(Like like)
    {
        return _dashboardService.SendLike(like);
    }

    public Task<bool> UpdateComments(CommentsResponse comment)
    {
        return _dashboardService.UpdateComments(new CommentsRequest()
        {
            Id = comment.Id,
            Date = comment.Date,
            Message = comment.Message,
            KudosId = comment.KudosId,
            FromPersonId = comment.FromPerson.Id
        });
    }

    public  Task<bool> DeleteComments(string kudosId, string commentId)
    {
        return _dashboardService.DeleteComments(kudosId, commentId);
    }
}
