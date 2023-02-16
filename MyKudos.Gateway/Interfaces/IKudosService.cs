using MyKudos.Gateway.Models;

namespace MyKudos.Gateway.Interfaces
{
    public interface IKudosService
    {
        Task<IEnumerable<Models.Kudos>> GetKudosAsync();

        Task<string> SendAsync(Models.KudosRequest kudos);

        Task<bool> SendLikeAsync(LikeGateway like);
    }
}