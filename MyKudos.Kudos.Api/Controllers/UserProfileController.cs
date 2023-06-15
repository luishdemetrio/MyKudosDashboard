using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;

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


    [HttpPost(Name = "PopulateUsers")]
    public bool PopulateUsers()
    {

        var graphUsers = _userProfileService.GetAllUsers();

        return true;

    }

    [HttpGet(Name = "GetUsers")]
    public List<UserProfile> GetUsers(string name)
    {

        return _userProfileService.GetUsers(name);

        

    }

}
