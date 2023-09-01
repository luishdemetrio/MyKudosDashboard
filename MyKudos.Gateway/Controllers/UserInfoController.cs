using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Domain.Models;
using MyKudos.Gateway.Interfaces;


namespace MyKudos.Gateway.Controllers;

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
    public async Task<UserProfile?> Get(Guid userId)
    {
        UserProfile result = null;

        var r =  await _userProfileService.GetUser(userId);

        if (r != null)
        {
            result = new UserProfile()
            {
                UserProfileId = r.UserProfileId,
                DisplayName = r.DisplayName,
                GivenName = r.GivenName,
                Mail = r.Mail,
                ManagerId = r.ManagerId,
                Photo = r.Photo,    
                Photo96x96 = r.Photo96x96,
                HasDirectReports = r.HasDirectReports
            };
        }

        return result;
    }
}
