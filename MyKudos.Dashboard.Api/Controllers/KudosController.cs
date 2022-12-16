using Microsoft.AspNetCore.Mvc;
using MyKudos.Dashboard.App.Interfaces;
using MyKudos.Dashboard.Domain.Models;


namespace MyKudos.Dashboard.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class KudosController : ControllerBase
{

    private readonly IDashboardKudosService _kudosService;

    public KudosController(IDashboardKudosService kudosService)
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