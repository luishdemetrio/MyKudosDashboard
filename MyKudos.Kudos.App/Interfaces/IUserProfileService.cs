using MyKudos.Kudos.Domain.Models;


namespace MyKudos.Kudos.App.Interfaces;

public interface IUserProfileService
{
    bool PopulateUserProfile();

    bool AddUser(UserProfile user);

    List<UserProfile> GetAllUsers();
}
