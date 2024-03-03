using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ScorePointsController : Controller
{
    private readonly IScorePointsRepository _scorePointsRepository;

    public ScorePointsController(IScorePointsRepository scorePointsRepository)
    {
        _scorePointsRepository = scorePointsRepository;
    }

    [HttpGet(Name ="GetScore")]
    public ScorePoints? Get()
    {
        return _scorePointsRepository.GetScore().FirstOrDefault();
    }

    [HttpPut(Name = "UpdateScore")]
    public bool Post([FromBody]ScorePoints scorePoints)
    {
        return _scorePointsRepository.UpdateScore(scorePoints);
    }
}
