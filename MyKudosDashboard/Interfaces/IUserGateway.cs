using MyKudos.Gateway.Domain.Models;

namespace MyKudosDashboard.Interfaces;

public interface IUserGateway
{
    Task<IEnumerable<Person>> GetUsers(string name);

    Task<string> GetUserPhoto(string userid);

    Task<UserProfile> GetUserInfo(string userid);
}
