using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Gateway.Interfaces;

public interface IKudosService
{
    Task<IEnumerable<Models.Kudos>> GetKudosAsync();

    Task<string> SendAsync(KudosLog kudos);

    Task<bool> LikeAsync(Kudos.Domain.Models.SendLike like);

    Task<bool> UndoLikeAsync(Kudos.Domain.Models.SendLike dislike);


}