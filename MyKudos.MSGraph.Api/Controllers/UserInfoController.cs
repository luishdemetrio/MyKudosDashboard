using Microsoft.AspNetCore.Mvc;
using MyKudos.MSGraph.Api.Interfaces;

namespace MyKudos.MSGraph.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserInfoController : Controller
{

    private readonly IGraphService _graphService;

    public UserInfoController(IGraphService graphService)
    {
        _graphService = graphService;
    }

    [HttpGet(Name = "GetUserInfo/{users}")]
    public Task<List<Models.GraphUser>> GetUserInfo([FromBody] string[] users)
    {
        return _graphService.GetUserInfo(users);
    }
}
