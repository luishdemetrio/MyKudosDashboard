using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Api.Controllers;


[ApiController]
[Route("[controller]")]

public class UserInfoController : Controller
{
    private readonly IUserProfileService _userProfileService;

    public UserInfoController(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
    }

    [HttpGet(Name = "GetUser")]
    public UserProfile? GetUser(Guid userid)
    {
        return _userProfileService.GetUser(userid);

    }
}
