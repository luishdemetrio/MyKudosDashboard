using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Api.Controllers;

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
