using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;


namespace MyKudosDashboard.Views;

public class KudosListView : IKudosListView
{

    private readonly IGatewayService _gatewayService;


    public KudosListView(IGatewayService gatewayService)
    {
        _gatewayService = gatewayService;

    }

    public async Task<bool> SendLikeAsync(Like like)
    {
        return await _gatewayService.SendLike(like);
    }


}
