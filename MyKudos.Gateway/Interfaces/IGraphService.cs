using MyKudos.Gateway.Models;

namespace MyKudos.Gateway.Interfaces;

public interface IGraphService
{
    Task<string> GetAppOnlyTokenAsync();
    
    Task<string> GetUserPhoto(string userid);
    
    Task<IEnumerable<GraphUserPhoto>> GetUserPhotos(string[] users);

    Task<GraphUsers> GetUsersAsync(string name);

    Task<List<GraphUser>> GetUserInfoAsync(string[] users);

    string GetUserManager(string userid);
}

