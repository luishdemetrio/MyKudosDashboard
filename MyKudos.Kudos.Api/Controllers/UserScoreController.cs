using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserScoreController : ControllerBase
{

    private readonly IUserScoreService _userScoreService;

    public UserScoreController(IUserScoreService userScoreService)
    {
        _userScoreService = userScoreService;
    }

    [HttpGet(Name = "GetUserScores")]
    public UserScore Get(string userId)
    {

        return _userScoreService.GetUserScore(userId);

    }


    [HttpPost(Name = "SetUserScore")]
    public UserScore? SetUserScore([FromBody] UserScore userScore)
    {

        return _userScoreService.SetUserScore(userScore);

    }
}