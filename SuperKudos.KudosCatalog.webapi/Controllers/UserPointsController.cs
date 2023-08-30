using Microsoft.AspNetCore.Mvc;
using SuperKudos.KudosCatalog.App.Interfaces;
using SuperKudos.KudosCatalog.Domain.Models;

namespace SuperKudos.KudosCatalog.webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserPointsController : Controller
{

    private readonly IUserPointsService _userPointsService;

    public UserPointsController(IUserPointsService userPointsService)
    {
        _userPointsService = userPointsService;
    }

    [HttpGet(Name = "GetUserPoints")]
    public UserPointScore Get(Guid userId)
    {

        return _userPointsService.GetUserScore(userId);

    }


}
