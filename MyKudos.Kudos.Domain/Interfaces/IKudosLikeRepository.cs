
namespace MyKudos.Kudos.Domain.Interfaces;

public interface IKudosLikeRepository
{
    bool Like(int kudosId, string personId);

    bool UndoLike(int kudosId, string personId);
}
