using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class UserPointsController : Controller
{

    private readonly IUserPointsService _userPointsService;

    public UserPointsController(IUserPointsService userPointsService)
    {
        _userPointsService = userPointsService;
    }

    [HttpGet("GetUserPoints/{userId},{justMyTeam},{sentOnYear}")]
    public async Task<UserPointScore> GetUserPoints(Guid userId, bool justMyTeam = false, int? sentOnYear = null)
    {
        return await _userPointsService.GetUserScoreAsync(userId, justMyTeam, sentOnYear);

    }

   
}