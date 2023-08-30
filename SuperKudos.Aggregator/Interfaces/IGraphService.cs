
using SuperKudos.KudosCatalog.Domain.Models;

namespace SuperKudos.Aggregator.Interfaces;

public interface IGraphService
{
    
    Task<string> GetUserPhoto(Guid userid);
    
    Task<IEnumerable<GraphUserPhoto>> GetUserPhotos(Guid[] users);

    Task<List<GraphUser>> GetUsers(string name);

    Task<List<GraphUser>> GetUserInfo(Guid[] users);

    Task<Guid[]?> GetUserManagerAsync(Guid[] userids);
}

public class GraphUsers
{
    public List<GraphUser> value { get; set; } = new();
}