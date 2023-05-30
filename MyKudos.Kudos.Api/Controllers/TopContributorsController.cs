using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TopContributorsController : Controller
{

    private readonly IUserPointsService _userPointsService;

    public TopContributorsController(IUserPointsService userPointsService)
    {
        _userPointsService = userPointsService;
    }

    [HttpGet(Name = "GetTopUserPoints")]
    public List<UserPoint> Get(int top)
    {

        return _userPointsService.GetTopUserScores(top);

    }
}
