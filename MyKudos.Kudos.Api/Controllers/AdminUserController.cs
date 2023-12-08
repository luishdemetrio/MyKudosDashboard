using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Data.Repository;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AdminUserController : Controller
{
    private IAdminUserRepository _repository;

    public AdminUserController(IAdminUserRepository repository)
    {
        _repository = repository;
    }
    [HttpGet("GetAdmins")]
    public IEnumerable<AdminUser> GetAdmins()
    {
        return _repository.GetAdmins();
    }

    [HttpGet("IsAdminUser/{userProfileId}")]
    public bool Get(Guid userProfileId)
    {
        return _repository.IsAdminUser(userProfileId);
    }

    [HttpPost(Name = "AddAdminUser")]
    public bool Post([FromBody] Guid userProfileId)
    {

        return _repository.Add(userProfileId);
    }

    [HttpDelete(Name = "RemoveAdminUser")]
    public bool Delete([FromBody] Guid userProfileId)
    {
        return _repository.Delete(userProfileId);
    }
    
    
}
