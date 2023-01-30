using MyKudos.Gateway.Models;

namespace MyKudos.Gateway.Interfaces;

public interface IGraphService
{
    
    Task<string> GetUserPhoto(string userid);
    
    Task<IEnumerable<GraphUserPhoto>> GetUserPhotos(string[] users);

    Task<GraphUsers> GetUsers(string name);

    Task<List<GraphUser>> GetUserInfo(string[] users);

    string GetUserManager(string userid);
}

