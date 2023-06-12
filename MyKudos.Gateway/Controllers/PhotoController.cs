using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Interfaces;

namespace MyKudos.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class PhotoController : Controller
{

    private IGraphService _graphService;
    private string _defaultProfilePicture;

    public PhotoController(IGraphService graphService, IConfiguration configuration)
    {
        _graphService = graphService;

        _defaultProfilePicture = configuration["DefaultProfilePicture"];
    }

    [HttpGet(Name = "GetUserphoto/{userid}")]        
    public async Task<string> GetUserPhoto(string userid)
    {
        
        string userProfile  = await _graphService.GetUserPhoto(userid);

        if (string.IsNullOrEmpty(userProfile))
        {
            return _defaultProfilePicture;
        }
        return $"data:image/png;base64,{userProfile}";

    }
}
