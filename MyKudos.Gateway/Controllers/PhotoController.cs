using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Interfaces;

namespace MyKudos.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class PhotoController : Controller
{

    private IGraphService _graphService;

    public PhotoController(IGraphService graphService)
    {
        _graphService = graphService;
    }

    [HttpGet(Name = "GetUserphoto/{userid}")]        
    public async Task<string> GetUserPhoto(string userid)
    {
        
        return $"data:image/png;base64,{await _graphService.GetUserPhoto(userid)}";

    }
}
