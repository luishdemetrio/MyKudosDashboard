using Microsoft.AspNetCore.Mvc;
using MyKudos.MSGraph.Api.Interfaces;
using MyKudos.MSGraph.Api.Models;

namespace MyKudos.MSGraph.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AllUsersController : Controller
{

    private readonly IGraphService _graphService;

    private readonly string[] _emailDomain;

    public AllUsersController(IGraphService graphService, IConfiguration configuration)
    {
        _graphService = graphService;

        _emailDomain = configuration["EmailDomain"].ToString().Split(",");
    }

    [HttpGet(Name = "GetAllUsers")]
    public async Task<IEnumerable<GraphUser>> GetAllUsers()
    {

        List<GraphUser> result = new();

        foreach (var domain in _emailDomain)
        {
            result.AddRange(await _graphService.GetAllUsers( domain));
        }

        return result;
    }
}
