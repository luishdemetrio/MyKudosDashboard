
namespace MyKudos.Gateway.Interfaces;

public interface IKudosService
{
    Task<IEnumerable<Kudos.Domain.Models.Kudos>> GetKudosAsync(int pageNumber, Guid? managerId = null);
    Task<IEnumerable<Kudos.Domain.Models.Kudos>> GetKudosFromMeAsync(string userId, int pageNumber, Guid? managerId = null);
    Task<IEnumerable<Kudos.Domain.Models.Kudos>> GetKudosToMeAsync(string userId, int pageNumber, Guid? managerId = null);
    
    Task<int> SendAsync(Kudos.Domain.Models.Kudos kudos);

    Task<bool> LikeAsync(Kudos.Domain.Models.SendLike like);

    Task<bool> UndoLikeAsync(Kudos.Domain.Models.SendLike dislike);

    Task<Kudos.Domain.Models.Kudos> GetKudosUser(int kudosId);

    Task<bool> UpdateKudos(Kudos.Domain.Models.KudosMessage kudos);

    Task<bool> DeleteKudos(int kudosId);
}