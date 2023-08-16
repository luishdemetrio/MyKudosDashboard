using MyKudos.Kudos.Domain.Models;


namespace MyKudos.Kudos.App.Interfaces;

public interface IUserProfileService
{
   

    bool AddUser(UserProfile user);

    List<UserProfile> GetAllUsers();

    List<UserProfile> GetUsers(string name);

    string? GetUserPhoto(Guid userid);

    List<UserProfile> GetUsers(Guid[] ids);


}
