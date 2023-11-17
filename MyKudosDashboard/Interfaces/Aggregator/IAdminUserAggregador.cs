using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces.Aggregator;

public interface IAdminUserAggregador
{
    Task<bool> Add(Guid userProfileId);
    Task<bool> Delete(Guid userProfileId);
    Task<bool> IsAdminUser(Guid userProfileId);
    Task<IEnumerable<Person>> GetAdminsUsers();
}
