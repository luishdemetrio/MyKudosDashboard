
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Domain.Interfaces;

public interface IKudosRepository
{

    Task<IEnumerable<Models.Kudos>> GetKudosAsync(int pageNumber = 1, int pageSize = 5);

    IEnumerable<Models.Kudos> GetUserKudos(Guid pUserId);

    Task<IEnumerable<Domain.Models.Kudos>> GetKudosFromMeAsync(Guid pUserId, int pageNumber = 1, int pageSize = 5);
    Task<IEnumerable<Domain.Models.Kudos>> GetKudosToMeAsync(Guid pUserId, int pageNumber = 1, int pageSize = 5);

    int Add(Models.Kudos kudos);

    Domain.Models.Kudos? GetKudos(int kudosId);

}
