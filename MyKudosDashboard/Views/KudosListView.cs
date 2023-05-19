﻿using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;


namespace MyKudosDashboard.Views;

public class KudosListView : IKudosListView
{

    private readonly IKudosGateway _gatewayService;


    public KudosListView(IKudosGateway gatewayService)
    {
        _gatewayService = gatewayService;

    }

    public async Task<bool> SendLikeAsync(Like like)
    {
        return await _gatewayService.Like(like);
    }

    public async Task<bool> SendUndoLikeAsync(Like like)
    {
        return await _gatewayService.UndoLike(like);
    }

    public async Task<IEnumerable<KudosResponse>> GetKudos(int pageNumber)
    {
        return await _gatewayService.GetKudos(pageNumber);
    }

}
