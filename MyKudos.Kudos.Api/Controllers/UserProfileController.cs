using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Interfaces;

namespace MyKudos.Kudos.Api.Controllers;


[ApiController]
[Route("[controller]")]
public class UserProfileController : Controller
{
    private readonly IUserProfileService _userProfileService;

    public UserProfileController(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
    }


    [HttpGet(Name = "PopulateUsers")]
    public bool GetAllUsers()
    {

        var graphUsers = _userProfileService.GetAllUsers();

        return true;

    }
}
