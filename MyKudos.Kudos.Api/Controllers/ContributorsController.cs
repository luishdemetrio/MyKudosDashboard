using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Api.Controllers;

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
