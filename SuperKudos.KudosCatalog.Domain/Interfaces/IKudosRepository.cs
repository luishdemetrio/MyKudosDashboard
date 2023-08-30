
using SuperKudos.KudosCatalog.Domain.Models;

namespace SuperKudos.KudosCatalog.Domain.Interfaces;

public interface IKudosRepository
{

    Task<IEnumerable<Kudos>> GetKudosAsync(int pageNumber = 1, int pageSize = 5);

    IEnumerable<Kudos> GetUserKudos(Guid pUserId);

    Task<IEnumerable<Kudos>> GetKudosFromMeAsync(Guid pUserId, int pageNumber = 1, int pageSize = 5);
    Task<IEnumerable<Kudos>> GetKudosToMeAsync(Guid pUserId, int pageNumber = 1, int pageSize = 5);

    int Add(Models.Kudos kudos);

    Domain.Models.Kudos? GetKudos(int kudosId);

}
