using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Models;

namespace MyKudos.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class LikesController : Controller
{

    private readonly IKudosService _kudosService;

    private IKudosQueue _kudosQueue;

    public LikesController(IKudosService kudosService, IKudosQueue kudosQueue)
    {

        _kudosService = kudosService;

        _kudosQueue = kudosQueue;
    }

    [HttpPost(Name = "SendLike")]
    public IActionResult SendLike([FromBody] LikeGateway like)
    {

        var kudos = _kudosService.SendLikeAsync(like);


         _kudosQueue.SendLikeAsync(like);

        return Ok();
    }
}
