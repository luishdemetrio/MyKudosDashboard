using Microsoft.AspNetCore.Mvc;
using MyKudos.Dashboard.App.Interface;
using MyKudos.Dashboard.Domain.Models;


namespace MyKudos.Dashboard.Api.Controllers;

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
    public IActionResult Post([FromBody] Kudos kudos)
    {

        _kudosService.Send(kudos);

        return Ok(kudos);
    }

}