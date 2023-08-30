using Microsoft.AspNetCore.Mvc;
using SuperKudos.KudosCatalog.App.Interfaces;

namespace SuperKudos.KudosCatalog.webapi.Controllers;

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
