using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Models;

namespace MyKudos.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class LikesController : Controller
{

    private readonly IKudosService _kudosService;


    public LikesController(IKudosService kudosService)
    {

        _kudosService = kudosService;


    }

    [HttpPost(Name = "SendLike")]
    public IActionResult SendLike([FromBody] LikeGateway like)
    {

        var kudos = _kudosService.SendLike(like); 

        return Ok();
    }
}
