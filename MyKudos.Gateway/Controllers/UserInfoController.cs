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

    private string _defaultProfilePicture;

    public UserInfoController(IUserProfileService userProfileService, IAdminUserService adminUserService,
                             IConfiguration configuration)
    {
        _userProfileService = userProfileService;
        _adminUserService = adminUserService;

        _defaultProfilePicture = configuration["DefaultProfilePicture"];
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
                Photo = userInfo.Photo != null ? $"data:image/png;base64,{userInfo.Photo}" : _defaultProfilePicture,
                Photo96x96 = userInfo.Photo96x96 != null ? $"data:image/png;base64,{userInfo.Photo96x96}" : _defaultProfilePicture,
                HasDirectReports = userInfo.HasDirectReports,
                IsAdmin = isAdminUser
            };
        }

        return result;
    }
}
