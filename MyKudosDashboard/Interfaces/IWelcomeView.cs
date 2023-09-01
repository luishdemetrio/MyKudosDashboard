
using MyKudos.Gateway.Domain.Models;

namespace MyKudosDashboard.Interfaces;

public interface IWelcomeView
{
    Task<UserProfile> GetUserInfo(string userId);

}
