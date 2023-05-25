
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.App.Interfaces;

public interface IKudosService
{
    public int Send(Domain.Models.Kudos kudos);

    public Task<IEnumerable<Domain.Models.Kudos>> GetKudos(int pageNumber, int pageSize);

    Task<IEnumerable<KudosGroupedByValue>> GetUserKudosByCategory(string pUserId);

    IEnumerable<Domain.Models.Kudos> GetUserKudos(string pUserId);

    public bool Like(int kudosId, string personId);
    public bool UndoLike(int kudosId, string personId);

    
}
