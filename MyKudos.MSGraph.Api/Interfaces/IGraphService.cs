using MyKudos.MSGraph.Api.Models;

namespace MyKudos.MSGraph.Api.Interfaces;

public interface IGraphService
{
    //Task<string> GetAppOnlyTokenAsync();
    
    Task<string> GetUserPhoto(string userid);
    
    Task<IEnumerable<GraphUserPhoto>> GetUserPhotos(string[] users);

    IEnumerable<GraphUser> GetUsers(string name, string emailDomain);

    Task<List<GraphUser>> GetUserInfo(string[] users);

    Task<string> GetUserManager(string userid);
}

