using Microsoft.AspNetCore.Mvc;
using MyKudos.MSGraph.Api.Interfaces;

namespace MyKudos.MSGraph.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ManagerController : Controller
{

    private readonly IGraphService _graphService;

    public ManagerController(IGraphService graphService)
    {
        _graphService = graphService;

    }
    [HttpGet(Name = "GetUserManager/{userid}")]
    public async Task<string> GetUserManager(string userid)
    {
        return await _graphService.GetUserManager(userid);
    }
}
