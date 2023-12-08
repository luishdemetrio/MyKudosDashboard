using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserPointsController : Controller
{

    private readonly IUserPointsRepository _userPointsRepository;

    public UserPointsController(IUserPointsRepository userPointsRepository)
    {
        _userPointsRepository = userPointsRepository;
    }

    [HttpGet("GetUserPoints/{userId}")]
    public UserPointScore GetUserPoints(Guid userId)
    {

        return _userPointsRepository.GetUserScore(userId);
    }

  
}
