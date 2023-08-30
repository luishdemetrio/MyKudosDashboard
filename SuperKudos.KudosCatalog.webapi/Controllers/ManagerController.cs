using Microsoft.AspNetCore.Mvc;
using SuperKudos.KudosCatalog.App.Interfaces;
using SuperKudos.KudosCatalog.Domain.Models;

namespace SuperKudos.KudosCatalog.webapi.Controllers;

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
