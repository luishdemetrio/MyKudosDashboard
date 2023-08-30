using Microsoft.AspNetCore.Mvc;
using SuperKudos.Aggregator.Interfaces;
using SuperKudos.Aggregator.Domain.Models;
using SuperKudos.KudosCatalog.Domain.Models;

namespace SuperKudos.Aggregator.Controllers;

[ApiController]
[Route("[controller]")]
public class ContributorsController : Controller
{
    private readonly IUserPointsService _topContributorsService  ;
   
    private readonly int _topContributors;

    private string _defaultProfilePicture;

    public ContributorsController(IUserPointsService topContributorsService, IConfiguration configuration)
    {
        _topContributorsService = topContributorsService;
        
        _topContributors = int.Parse(configuration["TopContributors"]);

        _defaultProfilePicture = configuration["DefaultProfilePicture"];
    }

    [HttpGet(Name = "GetTopContributors")]
    public async Task<IEnumerable<TopContributors>> Get()
    {

        var scores = await _topContributorsService.GetTopUserScoresAsync(_topContributors);

        var result = from score in scores
                     select new TopContributors()
                     {
                         Name = score.DisplayName,
                         Photo = string.IsNullOrEmpty(score.Photo)? _defaultProfilePicture : $"data:image/png;base64,{score.Photo}",
                         Score = score.TotalPoints
                     };

        return result;

    }
}
