using SuperKudos.KudosCatalog.Domain.Models;

namespace SuperKudos.Aggregator.Interfaces;

public interface IUserProfileService
{

    Task<List<UserProfile>> GetUsers(string name);
    
    Task<string> GetUserPhoto(Guid userid);

    Task<List<UserProfile>> GetManagers(Guid[] ids);
}
