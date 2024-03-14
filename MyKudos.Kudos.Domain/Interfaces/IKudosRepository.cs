
namespace MyKudos.Kudos.Domain.Interfaces;

public interface IKudosRepository
{

    Task<IEnumerable<Models.Kudos>> GetKudosAsync(int pageNumber = 1, int pageSize = 5, Guid? managerId = null,
                                                  int? year = null);

    Task<IEnumerable<Domain.Models.Kudos>> GetKudosFromMeAsync(Guid pUserId, int pageNumber = 1, int pageSize = 5,
                                                               Guid? managerId = null,
                                                                        int? year = null);

    Task<IEnumerable<Domain.Models.Kudos>> GetKudosToMeAsync(Guid pUserId, int pageNumber = 1, int pageSize = 5, 
                                                             Guid? managerId = null, int? year = null);

    IEnumerable<Models.Kudos> GetUserKudos(Guid pUserId,int? year = null);
    
    int Add(Models.Kudos kudos);

    Domain.Models.Kudos? GetKudos(int kudosId);
    bool UpdateMessage(int kudosId, string? message);
    bool Delete(int kudosId);
}
