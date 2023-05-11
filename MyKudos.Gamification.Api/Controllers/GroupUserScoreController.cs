using Microsoft.AspNetCore.Mvc;
using MyKudos.Gamification.App.Interfaces;
using MyKudos.Gamification.Domain.Models;

namespace MyKudos.Gamification.Api.Controllers;


[ApiController]
[Route("[controller]")]
public class GroupUserScoreController : Controller
{
    private readonly IUserScoreService _userScoreService;

    public GroupUserScoreController(IUserScoreService userScoreService)
    {
        _userScoreService = userScoreService;
    }




    [HttpPost(Name = "SetGroupUserScore")]
    public bool SetUserScore([FromBody] UserScore userScore)
    {
        return _userScoreService.UpdateGroupScore(userScore);

    }
}
