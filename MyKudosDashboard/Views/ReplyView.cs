using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Views;

public class ReplyView : IReplyView
{

    private IGatewayService _dashboardService;

    public ReplyView(IGatewayService dashboardService)
    {

        _dashboardService = dashboardService;

    }

    public Task<bool> SendLikeAsync(LikeComment like)
    {
        return _dashboardService.LikeComment(like);
    }

    public Task<bool> SendUndoLikeAsync(LikeComment like)
    {
        return _dashboardService.UndoLikeComment(like);
    }
}
