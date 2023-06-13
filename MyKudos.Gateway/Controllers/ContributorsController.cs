using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Domain.Models;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class ContributorsController : Controller
{
    private readonly IUserPointsService _topContributorsService  ;
    private readonly IGraphService _graphService;

    private readonly int _topContributors;

    public ContributorsController(IUserPointsService topContributorsService, IGraphService graphService, IConfiguration configuration)
    {
        _topContributorsService = topContributorsService;
        _graphService = graphService;

        _topContributors = int.Parse(configuration["TopContributors"]);
    }

    [HttpGet(Name = "GetTopContributors")]
    public async Task<IEnumerable<TopContributors>> Get()
    {

        var scores = await _topContributorsService.GetTopUserScoresAsync(_topContributors);

        var userIds = scores.Select(s => s.UserId).Distinct().ToArray();

        List<GraphUser> users = await _graphService.GetUserInfo(userIds).ConfigureAwait(true);

        var photos = await _graphService.GetUserPhotos(userIds).ConfigureAwait(true);

        var result = from score in scores
                     join photo in photos
                        on score.UserId equals photo.id
                     join user in users
                        on score.UserId equals user.Id
                     select new TopContributors()
                     {
                         Name = user.DisplayName,
                         Photo = $"data:image/png;base64,{photo.photo}",
                         Score = score.TotalPoints
                     };

        return result;

    }
}
