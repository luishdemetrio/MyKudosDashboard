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

    [HttpPost(Name ="SendLike")]
    public IActionResult Post(SendLike like)
    {

        var r = _kudosService.SendLike(like.KudosId, like.PersonId);

        return Ok(r);
    }
}
