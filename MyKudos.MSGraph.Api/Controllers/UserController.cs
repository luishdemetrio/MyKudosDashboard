using Microsoft.AspNetCore.Mvc;
using MyKudos.MSGraph.Api.Interfaces;
using MyKudos.MSGraph.Api.Models;

namespace MyKudos.MSGraph.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    
    private readonly IGraphService _graphService;

    public UserController(IGraphService graphService)
    {
        _graphService= graphService;
    }

    [HttpGet(Name = "GetUser/{name}")]
    public Task<GraphUsers> GetUsers(string name)
    {
        return _graphService.GetUsers(name);
    }

   
}