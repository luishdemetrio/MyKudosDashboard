using Microsoft.AspNetCore.Mvc;
using MyKudos.MSGraph.Api.Interfaces;
using MyKudos.MSGraph.Api.Models;

namespace MyKudos.MSGraph.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    
    private readonly IGraphService _graphService;

    private readonly string[] _emailDomain;

    public UserController(IGraphService graphService, IConfiguration configuration)
    {
        _graphService= graphService;

        _emailDomain = configuration["EmailDomain"].ToString().Split(",");
    }

    [HttpGet(Name = "GetUser/{name}")]
    public IEnumerable<GraphUser> GetUsers(string name)
    {

        List<GraphUser> result = new();

        foreach (var domain in _emailDomain)
        {
            result.AddRange(_graphService.GetUsers(name, domain));
        }

        return result; 
    }

   
}