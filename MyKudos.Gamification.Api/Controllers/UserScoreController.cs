using Microsoft.AspNetCore.Mvc;
using MyKudos.Gamification.App.Interfaces;
using MyKudos.Gamification.Domain.Models;

namespace MyKudos.Gamification.Api.Controllers;

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
    public bool SetUserScore([FromBody] UserScore userScore)
    {

        return  _userScoreService.SetUserScore(userScore);

    }
}