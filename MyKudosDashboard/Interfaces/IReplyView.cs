using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface IReplyView
{
    Task<bool> SendLikeAsync(LikeComment like);
    Task<bool> SendUndoLikeAsync(LikeComment like);

}
