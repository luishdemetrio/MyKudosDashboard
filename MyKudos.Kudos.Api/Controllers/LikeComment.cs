using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LikeCommentController : Controller
{

    private readonly IKudosService _kudosService;

    public LikeCommentController(IKudosService kudosService)
    {
        _kudosService = kudosService;
    }

    [HttpPost(Name = "LikeComment")]
    public IActionResult Post(SendLike like)
    {

        var r = _kudosService.LikeComment(like.KudosId, like.FromPersonId);

        return Ok(r);
    }

    [HttpDelete(Name = "UndoLikeComment")]
    public IActionResult Delete([FromBody] SendLike like)
    {
        var r = _kudosService.UndoLikeComment(like.KudosId, like.FromPersonId);

        return Ok(r);



    }
}
