using Microsoft.AspNetCore.Mvc;
using SuperKudos.KudosCatalog.App.Interfaces;
using SuperKudos.KudosCatalog.Domain.Models;

namespace SuperKudos.KudosCatalog.webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class LikeCommentController : Controller
{

    private readonly ICommentsService _commentsService;

    public LikeCommentController(ICommentsService commentsService)
    {
        _commentsService = commentsService;
    }

    [HttpPost(Name = "LikeComment")]
    public IActionResult Post(SendLike like)
    {

        var r = _commentsService.LikeComment(like.KudosId, like.FromPersonId);

        return Ok(r);
    }

    [HttpDelete(Name = "UndoLikeComment")]
    public IActionResult Delete([FromBody] SendLike like)
    {
        var r = _commentsService.UndoLikeComment(like.KudosId, like.FromPersonId);

        return Ok(r);
    }
}
