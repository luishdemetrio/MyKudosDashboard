

namespace MyKudos.Kudos.Data.Interfaces;

public interface IKudosRepository
{

    Task<IEnumerable<MyKudos.Kudos.Domain.Models.Kudos>> GetKudosAsync(int pageNumber = 1, int pageSize = 5);

    IEnumerable<MyKudos.Kudos.Domain.Models.Kudos> GetUserKudos(Guid pUserId);

    Task<IEnumerable<Domain.Models.Kudos>> GetKudosFromMeAsync(Guid pUserId, int pageNumber = 1, int pageSize = 5);
    Task<IEnumerable<Domain.Models.Kudos>> GetKudosToMeAsync(Guid pUserId, int pageNumber = 1, int pageSize = 5);

    int Add(MyKudos.Kudos.Domain.Models.Kudos kudos);

}
