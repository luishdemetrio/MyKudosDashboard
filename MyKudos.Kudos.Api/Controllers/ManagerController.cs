using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ManagerController : Controller
{
    private readonly IUserProfileService _userProfileService;

    public ManagerController(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
    }


    
    [HttpPost(Name = "GetManagers")]
    public List<UserProfile> GetUsers([FromBody] Guid[] ids)
    {

        return _userProfileService.GetUsers(ids);

    }

}
