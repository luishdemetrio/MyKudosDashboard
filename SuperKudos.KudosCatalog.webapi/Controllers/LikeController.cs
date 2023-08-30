using Microsoft.AspNetCore.Mvc;
using SuperKudos.KudosCatalog.App.Interfaces;
using SuperKudos.KudosCatalog.Domain.Models;

namespace SuperKudos.KudosCatalog.webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class LikeController : Controller
{

    private readonly IKudosService _kudosService;

    public LikeController(IKudosService kudosService)
    {
        _kudosService = kudosService;
    }

    [HttpPost(Name ="Like")]
    public IActionResult Post(SendLike like)
    {

        var r = _kudosService.Like(like.KudosId, like.FromPersonId);

        return Ok(r);
    }

    [HttpDelete(Name = "UndoLike")]
    public IActionResult Delete([FromBody] SendLike like)
    {
        var r = _kudosService.UndoLike(like.KudosId, like.FromPersonId);

        return Ok(r);



    }

}



