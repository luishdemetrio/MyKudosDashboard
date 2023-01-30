using Microsoft.AspNetCore.Mvc;
using MyKudos.MSGraph.Api.Interfaces;

namespace MyKudos.MSGraph.Api.Controllers;


[ApiController]
[Route("[controller]")]
public class PhotoController : Controller
{
    private readonly IGraphService _graphService;

    public PhotoController(IGraphService graphService)
    {
        _graphService = graphService;
    }

    [HttpGet(Name = "GetUserPhoto/{usersId}")]
    public Task<string> GetUserPhoto(string userid)
    {
        return _graphService.GetUserPhoto(userid);
    }
}
