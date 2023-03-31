
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Domain.Interfaces;

public interface IKudosRepository
{

    IEnumerable<KudosLog> GetKudos();

    Guid Add(KudosLog kudos);

    int SendLike(string kudosId, string personId);

    bool SendComments(string kudosId, string commentId);

    bool DeleteComments(string kudosId, Guid commentId);
}
