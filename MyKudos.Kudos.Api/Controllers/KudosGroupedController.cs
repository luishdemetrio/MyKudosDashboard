using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class KudosGroupedController : Controller
{
    private readonly IKudosService _kudosService;

    

    public KudosGroupedController(IKudosService kudosService)
    {
        _kudosService = kudosService;

    }

    [HttpGet(Name = "Get")]
    public Task<IEnumerable<KudosGroupedByValue>> Get(string pUserId)
    {
       

        return _kudosService.GetUserKudosByCategory(pUserId);
    }
}
