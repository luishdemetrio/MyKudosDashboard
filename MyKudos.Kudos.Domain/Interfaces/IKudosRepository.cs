
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Domain.Interfaces;

public interface IKudosRepository
{

    Task<IEnumerable<KudosLog>> GetKudosAsync(int pageNumber = 1, int pageSize = 5);

    IEnumerable<KudosLog> GetUserKudos(string pUserId);

    Guid Add(KudosLog kudos);

    bool Like(string kudosId, string personId);

    bool UndoLike(string kudosId, string personId);

    bool SendComments(string kudosId, string commentId);

    bool DeleteComments(string kudosId, Guid commentId);
}
