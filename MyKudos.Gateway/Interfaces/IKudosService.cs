using MyKudos.Gateway.Models;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Gateway.Interfaces;

public interface IKudosService
{
    Task<IEnumerable<Models.Kudos>> GetKudosAsync();

    Task<string> SendAsync(Models.KudosRequest kudos);

    Task<int> SendLikeAsync(LikeGateway like);

    Task<string> SendCommentsAsync(Comments comment);

    Task<IEnumerable<Comments>> GetComments(string kudosId);

    

}