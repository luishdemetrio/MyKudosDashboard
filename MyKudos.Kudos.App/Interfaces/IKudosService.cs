
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.App.Interfaces;

public interface IKudosService
{
    public int Send(Domain.Models.Kudos kudos);

    public Task<IEnumerable<Domain.Models.Kudos>> GetKudos(int pageNumber, int pageSize);

    Task<IEnumerable<KudosGroupedByValue>> GetUserKudosByCategory(string pUserId);

    IEnumerable<Domain.Models.Kudos> GetUserKudos(string pUserId);

    Task<IEnumerable<Domain.Models.Kudos>> GetKudosFromMeAsync(string pUserId, int pageNumber = 1, int pageSize = 5);
    Task<IEnumerable<Domain.Models.Kudos>> GetKudosToMeAsync(string pUserId, int pageNumber = 1, int pageSize = 5);

    public bool Like(int kudosId, string personId);
    public bool UndoLike(int kudosId, string personId);

    
}
