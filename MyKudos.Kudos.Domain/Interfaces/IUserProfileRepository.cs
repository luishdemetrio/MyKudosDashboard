using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Domain.Interfaces;

public interface IUserProfileRepository
{
  
    bool Add(UserProfile user);
    bool AddRange(List<UserProfile> user);

    List<UserProfile> GetAll();

    List<UserProfile> GetUsers(string name);

    string? GetUserPhoto(Guid userid);

    bool Truncate();

    bool PopulateUserProfile(List<UserProfile> users);

    List<UserProfile> GetUsers(Guid[] ids);
}
