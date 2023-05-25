
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Domain.Interfaces;

public interface IKudosRepository
{

    Task<IEnumerable<Models.KudosLog>> GetKudosAsync(int pageNumber = 1, int pageSize = 5);

    IEnumerable<Models.KudosLog> GetUserKudos(string pUserId);

    int Add(Models.KudosLog kudos);

}
