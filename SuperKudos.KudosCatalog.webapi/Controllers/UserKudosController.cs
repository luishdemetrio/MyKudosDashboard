using Microsoft.AspNetCore.Mvc;
using SuperKudos.KudosCatalog.App.Interfaces;
using SuperKudos.KudosCatalog.Domain.Models;

namespace SuperKudos.KudosCatalog.webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserKudosController : Controller
{
    private readonly IKudosService _kudosService;

    

    public UserKudosController(IKudosService kudosService)
    {
        _kudosService = kudosService;

    }

    [HttpGet(Name = "Get")]
    public Task<IEnumerable<KudosGroupedByValue>> Get(Guid userId)
    {
       

        return _kudosService.GetUserKudosByCategory(userId);
    }
}
