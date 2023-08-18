using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Gateway.Interfaces;

public interface IUserProfileService
{

    Task<List<UserProfile>> GetUsers(string name);
    
    Task<string> GetUserPhoto(Guid userid);

    Task<List<UserProfile>> GetManagers(Guid[] ids);
}
