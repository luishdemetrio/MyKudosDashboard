using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Interfaces;

namespace MyKudos.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class AdminUserController : Controller
{
    private IAdminUserService _adminUserService;

    public AdminUserController(IAdminUserService adminUserService)
    {
        _adminUserService = adminUserService;
    }

    [HttpGet("GetAdmins")]
    public async Task<IEnumerable<Gateway.Domain.Models.Person>> GetAdmins()
    {
        var result = new List<Gateway.Domain.Models.Person>();

        var admins =  await _adminUserService.GetAdminsUsers();

        foreach (var admin in admins)
        {
            if (admin.Person != null)
                result.Add(new Domain.Models.Person()
                {
                    Id = admin.UserProfileId,
                    GivenName = admin.Person.GivenName,
                    Name = admin.Person.DisplayName,
                    Photo = admin.Person.Photo
                });
        }

        return result;

    }

    [HttpGet("IsAdminUser/{userProfileId}")]
    public async Task<bool> Get(Guid userProfileId)
    {
        return await _adminUserService.IsAdminUser(userProfileId);
    }

    [HttpPost(Name = "AddAdminUser")]
    public async Task<bool> PostAsync([FromBody] Guid userProfileId)
    {

        return await _adminUserService.Add(userProfileId);
    }

    [HttpDelete(Name = "RemoveAdminUser")]
    public async Task<bool> DeleteAsync([FromBody] Guid userProfileId)
    {
        return await _adminUserService.Delete(userProfileId);
    }
}
