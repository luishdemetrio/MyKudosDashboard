using SuperKudos.KudosCatalog.Domain.Models;

namespace SuperKudos.Aggregator.Interfaces;

public interface IKudosService
{
    Task<IEnumerable<Kudos>> GetKudosAsync(int pageNumber);
    Task<IEnumerable<Kudos>> GetKudosFromMeAsync(string userId, int pageNumber);
    Task<IEnumerable<Kudos>> GetKudosToMeAsync(string userId, int pageNumber);

    Task<int> SendAsync(Kudos kudos);

    Task<bool> LikeAsync(SendLike like);

    Task<bool> UndoLikeAsync(SendLike dislike);

    Task<Kudos> GetKudosUser(int kudosId);
}