
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.App.Interfaces;

public interface IKudosService
{
    public int Send(Domain.Models.Kudos kudos);

    public Task<IEnumerable<Domain.Models.Kudos>> GetKudos(int pageNumber, int pageSize, 
                                                           Guid? managerId = null, int? year = null);

    Task<IEnumerable<KudosGroupedByValue>> GetUserKudosByCategory(Guid pUserId);

    IEnumerable<Domain.Models.Kudos> GetUserKudos(Guid pUserId, int? year = null);

    Task<IEnumerable<Domain.Models.Kudos>> GetKudosFromMeAsync(Guid pUserId, int pageNumber = 1, int pageSize = 5, Guid? managerId = null, int? year = null);
    Task<IEnumerable<Domain.Models.Kudos>> GetKudosToMeAsync(Guid pUserId, int pageNumber = 1, int pageSize = 5, Guid? managerId = null, int? year = null);

    public bool Like(int kudosId, Guid personId);
    public bool UndoLike(int kudosId, Guid personId);

    Domain.Models.Kudos GetKudos(int kudosId);
    Task<IEnumerable<Domain.Models.Kudos>> GetKudosByName(string name, int pageSize, int fromNumberOfDays);

    bool UpdateKudos(int kudosId, string? message);
    bool DeleteKudos(int kudosId);
}
