using Microsoft.AspNetCore.Mvc;
using SuperKudos.KudosCatalog.App.Interfaces;
using SuperKudos.KudosCatalog.Domain.Models;

namespace SuperKudos.KudosCatalog.webapi.Controllers;

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
