using Microsoft.AspNetCore.Mvc;
using SuperKudos.KudosCatalog.App.Interfaces;

namespace SuperKudos.KudosCatalog.webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class PhotoController : Controller
{
    private readonly IUserProfileService _userProfileService;

    public PhotoController(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
    }




    [HttpGet(Name = "GetUserPhoto")]
    public string? GetUserPhoto(Guid userid)
    {

        return _userProfileService.GetUserPhoto(userid);

    }

}
