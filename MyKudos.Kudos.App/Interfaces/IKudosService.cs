
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.App.Interfaces;

public interface IKudosService
{
    public Guid Send(KudosLog kudos);

    public IEnumerable<KudosLog> GetKudos();

    public bool SendLike(string kudosId, string personId);
}
