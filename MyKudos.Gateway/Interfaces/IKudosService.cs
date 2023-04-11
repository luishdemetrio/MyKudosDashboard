using MyKudos.Gateway.Models;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Gateway.Interfaces;

public interface IKudosService
{
    Task<IEnumerable<Models.Kudos>> GetKudosAsync();

    Task<string> SendAsync(Models.KudosRequest kudos);

    Task<int> SendLikeAsync(SendLike like);

    Task<string> SendCommentsAsync(Comments comment);

    Task<IEnumerable<Comments>> GetComments(string kudosId);

    Task<bool> UpdateComments(Comments comments);

    Task<bool> DeleteComments(string kudosId, string commentId);

}