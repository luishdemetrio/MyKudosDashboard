using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Views;

public class KudosListView : IKudosListView
{
    private readonly IGatewayService _gatewayService;

    public KudosListView(IGatewayService gatewayService)
    {
        _gatewayService= gatewayService;
    }

    public bool SendLike(Like like)
    {
        return _gatewayService.SendLike(like);
    }
}
