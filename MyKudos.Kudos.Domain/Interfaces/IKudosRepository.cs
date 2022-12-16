
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Domain.Interfaces;

public interface IKudosRepository
{

    IEnumerable<KudosLog> GetKudos();

    void Add(KudosLog kudos);
}
