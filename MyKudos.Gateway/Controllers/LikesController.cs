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
    public async Task<IActionResult> SendLikeAsync([FromBody] LikeGateway like)
    {

        var sign = await _kudosService.SendLikeAsync(like).ConfigureAwait(false);


         await _kudosQueue.SendLikeAsync(like, sign);

        return Ok();
    }
}
