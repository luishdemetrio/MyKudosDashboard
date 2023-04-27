using Microsoft.AspNetCore.Mvc;
using MyKudos.MSGraph.Api.Interfaces;
using MyKudos.MSGraph.Api.Models;

namespace MyKudos.MSGraph.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    
    private readonly IGraphService _graphService;

    private readonly string _emailDomain;

    public UserController(IGraphService graphService, IConfiguration configuration)
    {
        _graphService= graphService;

        _emailDomain = configuration["EmailDomain"];
    }

    [HttpGet(Name = "GetUser/{name}")]
    public Task<GraphUsers> GetUsers(string name)
    {
        return _graphService.GetUsers(name, _emailDomain);
    }

   
}