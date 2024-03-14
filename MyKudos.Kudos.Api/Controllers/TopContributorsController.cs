using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TopContributorsController : Controller
{

    private readonly IUserPointsRepository _repository;

    public TopContributorsController(IUserPointsRepository repository)
    {
        _repository = repository;
    }

    [HttpGet(Name = "GetTopUserPoints")]
    public List<UserPoint> Get(int top, Guid? managerId, int? sentOnYear = null)
    {
        return _repository.GetTopUserScores(top, managerId, sentOnYear).Where(t => t.TotalPoints > 0).ToList();
    }
}
