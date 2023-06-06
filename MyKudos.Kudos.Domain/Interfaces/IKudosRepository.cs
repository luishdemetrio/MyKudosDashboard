
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Domain.Interfaces;

public interface IKudosRepository
{

    Task<IEnumerable<Models.Kudos>> GetKudosAsync(int pageNumber = 1, int pageSize = 5);

    IEnumerable<Models.Kudos> GetUserKudos(string pUserId);

    Task<IEnumerable<Domain.Models.Kudos>> GetKudosFromMeAsync(string pUserId, int pageNumber = 1, int pageSize = 5);
    Task<IEnumerable<Domain.Models.Kudos>> GetKudosToMeAsync(string pUserId, int pageNumber = 1, int pageSize = 5);

    int Add(Models.Kudos kudos);

}
