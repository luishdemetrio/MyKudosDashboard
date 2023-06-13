
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.App.Interfaces;

public interface IKudosService
{
    public int Send(Domain.Models.Kudos kudos);

    public Task<IEnumerable<Domain.Models.Kudos>> GetKudos(int pageNumber, int pageSize);

    Task<IEnumerable<KudosGroupedByValue>> GetUserKudosByCategory(Guid pUserId);

    IEnumerable<Domain.Models.Kudos> GetUserKudos(Guid pUserId);

    Task<IEnumerable<Domain.Models.Kudos>> GetKudosFromMeAsync(Guid pUserId, int pageNumber = 1, int pageSize = 5);
    Task<IEnumerable<Domain.Models.Kudos>> GetKudosToMeAsync(Guid pUserId, int pageNumber = 1, int pageSize = 5);

    public bool Like(int kudosId, Guid personId);
    public bool UndoLike(int kudosId, Guid personId);

    
}
