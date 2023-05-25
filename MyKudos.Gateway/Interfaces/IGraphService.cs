
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Gateway.Interfaces;

public interface IGraphService
{
    
    Task<string> GetUserPhoto(string userid);
    
    Task<IEnumerable<GraphUserPhoto>> GetUserPhotos(string[] users);

    Task<GraphUsers> GetUsers(string name);

    Task<List<GraphUser>> GetUserInfo(string[] users);

    Task<string> GetUserManagerAsync(string userid);
}

public class GraphUsers
{
    public List<GraphUser> value { get; set; } = new();
}