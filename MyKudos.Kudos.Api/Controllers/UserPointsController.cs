using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Api.Controllers;

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
