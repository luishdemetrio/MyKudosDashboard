using MyKudos.Kudos.Domain.Models;


namespace MyKudos.Kudos.Data.Interfaces;

public interface IUserProfileRepository
{
    bool Add(UserProfile user);

    List<UserProfile> GetAll();
}
