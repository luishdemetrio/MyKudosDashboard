
namespace SuperKudos.KudosCatalog.Domain.Interfaces;

public interface IKudosLikeRepository
{
    bool Like(int kudosId, Guid personId);

    bool UndoLike(int kudosId, Guid personId);
}
