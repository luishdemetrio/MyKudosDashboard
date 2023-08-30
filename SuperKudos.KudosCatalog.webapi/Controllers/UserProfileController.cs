using Microsoft.AspNetCore.Mvc;
using SuperKudos.KudosCatalog.App.Interfaces;
using SuperKudos.KudosCatalog.Domain.Models;

namespace SuperKudos.KudosCatalog.webapi.Controllers;


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
