using SuperKudos.KudosCatalog.Domain.Models;



namespace SuperKudos.KudosCatalog.App.Interfaces;

public interface IUserProfileService
{
   

    bool AddUser(UserProfile user);

    List<UserProfile> GetAllUsers();

    List<UserProfile> GetUsers(string name);

    string? GetUserPhoto(Guid userid);

    List<UserProfile> GetUsers(Guid[] ids);


}
