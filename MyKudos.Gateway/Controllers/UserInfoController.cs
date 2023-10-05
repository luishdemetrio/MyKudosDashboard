using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Domain.Models;
using MyKudos.Gateway.Interfaces;


namespace MyKudos.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class UserInfoController : Controller
{
    private readonly IUserProfileService _userProfileService;

    private readonly IAdminUserService _adminUserService;

    public UserInfoController(IUserProfileService userProfileService, IAdminUserService adminUserService)
    {
        _userProfileService = userProfileService;
        _adminUserService = adminUserService;
    }

    [HttpGet(Name = "GetUser")]
    public async Task<UserProfile?> Get(Guid userId)
    {
        UserProfile result = null;

        var userInfoTask =  _userProfileService.GetUser(userId);

        var adminInfoTask = _adminUserService.IsAdminUser(userId);

        await Task.WhenAll(userInfoTask, adminInfoTask);

        var userInfo = await userInfoTask;
        var isAdminUser = await adminInfoTask;

        if (userInfo != null)
        {
            result = new UserProfile()
            {
                UserProfileId = userInfo.UserProfileId,
                DisplayName = userInfo.DisplayName,
                GivenName = userInfo.GivenName,
                Mail = userInfo.Mail,
                ManagerId = userInfo.ManagerId,
                Photo = userInfo.Photo,    
                Photo96x96 = userInfo.Photo96x96,
                HasDirectReports = userInfo.HasDirectReports,
                IsAdmin = isAdminUser
            };
        }

        return result;
    }
}
