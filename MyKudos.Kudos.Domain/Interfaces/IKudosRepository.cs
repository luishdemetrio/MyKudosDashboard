
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Domain.Interfaces;

public interface IKudosRepository
{

    Task<IEnumerable<Models.Kudos>> GetKudosAsync(int pageNumber = 1, int pageSize = 5);

    IEnumerable<Models.Kudos> GetUserKudos(string pUserId);

    int Add(Models.Kudos kudos);

}
