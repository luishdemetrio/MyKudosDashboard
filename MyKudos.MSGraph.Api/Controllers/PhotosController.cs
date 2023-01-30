using Microsoft.AspNetCore.Mvc;
using MyKudos.MSGraph.Api.Interfaces;
using MyKudos.MSGraph.Api.Models;

namespace MyKudos.MSGraph.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PhotosController : Controller
{
    private readonly IGraphService _graphService;

    public PhotosController(IGraphService graphService)
    {
        _graphService = graphService;
    }

    [HttpGet(Name = "GetUserPhotos/{usersId}")]
    public Task<IEnumerable<GraphUserPhoto>> GetUserPhotos([FromBody] string[] usersId)
    {
        return _graphService.GetUserPhotos(usersId);
    }

  
}
