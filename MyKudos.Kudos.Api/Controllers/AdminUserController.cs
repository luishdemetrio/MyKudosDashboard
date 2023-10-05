using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Data.Repository;

namespace MyKudos.Kudos.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AdminUserController : Controller
{
    private IAdminUserService _adminUserService;

    public AdminUserController(IAdminUserService adminUserService)
    {
            _adminUserService = adminUserService;
    }

    [HttpGet(Name = "IsAdminUser")]
    public bool Get(Guid userProfileId)
    {
        return _adminUserService.IsAdminUser(userProfileId);
    }

    [HttpPost(Name = "AddAdminUser")]
    public bool Post(Guid userProfileId)
    {

        return _adminUserService.Add(userProfileId);
    }

    [HttpDelete(Name = "RemoveAdminUser")]
    public bool Delete(Guid userProfileId)
    {
        return _adminUserService.Delete(userProfileId);
    }
}
