using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class ScorePointsController : Controller
{

    private readonly IScorePointsService _scorePointsService;

    public ScorePointsController(IScorePointsService scorePointsService)
    {
        _scorePointsService = scorePointsService;
    }


    [HttpGet(Name ="GetScore")]
    public async Task<Gateway.Domain.Models.Points> Get()
    {
        Gateway.Domain.Models.Points points = new();

        var score =  await _scorePointsService.GetScore();

        if (score != null)
        {
            points.ScorePointsId = score.ScorePointsId;
            points.KudosReceived = score.KudosReceived;
            points.KudosSent = score.KudosSent;
            points.LikesSent = score.LikesSent;
            points.LikesReceived = score.LikesReceived;
            points.CommentsReceived = score.CommentsReceived;
            points.CommentsSent = score.CommentsSent;
        }

        return points;

    }

    [HttpPut(Name = "UpdateScore")]
    public async Task<IActionResult> Post([FromBody] Gateway.Domain.Models.Points scorePoints)
    {

        var score = new ScorePoints()
        {
            ScorePointsId = scorePoints.ScorePointsId,
            KudosSent = scorePoints.KudosSent,
            KudosReceived = scorePoints.KudosReceived,
            LikesSent = scorePoints.LikesSent,
            LikesReceived = scorePoints.LikesReceived,
            CommentsReceived = scorePoints.CommentsReceived,
            CommentsSent = scorePoints.CommentsSent
        };

        if (await _scorePointsService.UpdateScore(score))
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }

}
