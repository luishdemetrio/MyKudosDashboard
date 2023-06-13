
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Gateway.Interfaces;

public interface IGraphService
{
    
    Task<string> GetUserPhoto(Guid userid);
    
    Task<IEnumerable<GraphUserPhoto>> GetUserPhotos(Guid[] users);

    Task<List<GraphUser>> GetUsers(string name);

    Task<List<GraphUser>> GetUserInfo(Guid[] users);

    Task<Guid> GetUserManagerAsync(Guid userid);
}

public class GraphUsers
{
    public List<GraphUser> value { get; set; } = new();
}