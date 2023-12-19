using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.App.Interfaces;

namespace MyKudos.Kudos.Api.Controllers;

[ApiController]
[Route("[controller]")]

public class KudosIdController : Controller
{
    private readonly IKudosService _kudosService;

    
    public KudosIdController(IKudosService kudosService)
    {
        _kudosService = kudosService;      
    }

    [HttpGet(Name = "GetKudosId")]
    public Domain.Models.Kudos Get(int kudosId)
    {
        return _kudosService.GetKudos(kudosId);
    }
}
