using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface IKudosListView
{

    Task<bool> SendLikeAsync(Like like);

    Task<bool> SendUndoLikeAsync(Like like);

    Task<IEnumerable<KudosResponse>> GetKudos(int pageNumber);



}
