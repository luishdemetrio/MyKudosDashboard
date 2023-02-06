using MyKudos.Gateway.Models;

namespace MyKudos.Gateway.Interfaces
{
    public interface IKudosService
    {
        IEnumerable<Models.Kudos> GetKudos();

        string Send(Models.KudosRequest kudos);

        bool SendLike(LikeGateway like);
    }
}