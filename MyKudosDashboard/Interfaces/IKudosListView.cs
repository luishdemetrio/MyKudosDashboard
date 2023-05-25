using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface IKudosListView
{

    Task<bool> SendLikeAsync(LikeGateway like);

    Task<bool> SendUndoLikeAsync(LikeGateway like);

    Task<IEnumerable<KudosResponse>> GetKudos(int pageNumber);



}
