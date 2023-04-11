using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Api.Controllers;

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



