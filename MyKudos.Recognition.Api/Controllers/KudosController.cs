using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Models;
using MyKudos.Recognition.App.Interfaces;
using MyKudos.Recognition.Domain.Models;


namespace MyKudos.Recognition.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class KudosController : ControllerBase
{

    private readonly IKudosService _kudosService;

    public KudosController(IKudosService kudosService)
    {
        _kudosService = kudosService;
    }

    [HttpPost]
    public IActionResult Post([FromBody] KudosLog kudos)
    {

        _kudosService.Send(kudos);

        return Ok(kudos);
    }

}