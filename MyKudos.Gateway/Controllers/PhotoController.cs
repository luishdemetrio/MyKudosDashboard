using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Interfaces;

namespace MyKudos.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class PhotoController : Controller
{

    private readonly IUserProfileService _userProfileService;
    private string _defaultProfilePicture;

    public PhotoController(IUserProfileService userProfileService, IConfiguration configuration)
    {
        _userProfileService = userProfileService;

        _defaultProfilePicture = configuration["DefaultProfilePicture"];
    }

    [HttpGet(Name = "GetUserphoto/{userid}")]        
    public async Task<string> GetUserPhoto(Guid userid)
    {
        
        string userProfile  = await _userProfileService.GetUserPhoto(userid);

        if (string.IsNullOrEmpty(userProfile))
        {
            return _defaultProfilePicture;
        }
        return $"data:image/png;base64,{userProfile}";

    }
}
