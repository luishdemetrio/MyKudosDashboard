using Microsoft.AspNetCore.Mvc;
using SuperKudos.Aggregator.Interfaces;
using SuperKudos.KudosCatalog.Domain.Models;

namespace SuperKudos.Aggregator.Controllers;

[ApiController]
[Route("[controller]")]
public class UserPointsController : Controller
{

    private readonly IUserPointsService _userPointsService;

    public UserPointsController(IUserPointsService userPointsService)
    {
        _userPointsService = userPointsService;
    }

    [HttpGet(Name = "getuserscore")]
    public async Task<UserPointScore> Get(Guid userId)
    {

        return await _userPointsService.GetUserScoreAsync(userId);

    }

}