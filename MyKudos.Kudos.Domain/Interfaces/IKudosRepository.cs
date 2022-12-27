
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Domain.Interfaces;

public interface IKudosRepository
{

    IEnumerable<KudosLog> GetKudos();

    bool Add(KudosLog kudos);
}
