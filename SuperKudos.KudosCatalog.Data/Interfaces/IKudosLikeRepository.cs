
namespace MyKudos.Kudos.Data.Interfaces;

public interface IKudosLikeRepository
{
    bool Like(int kudosId, Guid personId);

    bool UndoLike(int kudosId, Guid personId);
}
