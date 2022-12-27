using MyKudos.Gateway.Models;

namespace MyKudos.Gateway.Interfaces;

public interface IGraphService
{
    Task<string> GetAppOnlyTokenAsync();
    
    Task<string> GetUserPhoto(string userid);
    
    Task<GraphUserPhotos> GetUserPhotos(GraphUsers users);
    
    Task<GraphUsers> GetUsers(string name);

    Task<List<GraphUser>> GetUserInfoAsync(string[] users);
}

