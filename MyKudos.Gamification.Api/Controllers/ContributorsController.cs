using Microsoft.AspNetCore.Mvc;
using MyKudos.Gamification.App.Interfaces;
using MyKudos.Gamification.Domain.Models;

namespace MyKudos.Gamification.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ContributorsController : Controller
{
    private readonly IUserScoreService _userScoreService;

    public ContributorsController(IUserScoreService userScoreService)
    {
        _userScoreService = userScoreService;
    }

   
    [HttpGet(Name = "GetTopUserScores")]
    public IEnumerable<UserScore> GetTopUserScores(int top)
    {

        return _userScoreService.GetTopUserScores(top);

    }
}
