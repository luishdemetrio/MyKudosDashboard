
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Domain.Interfaces;

public interface IAdminUserRepository
{
    bool Add(Guid userProfileId);
    bool Delete(Guid userProfileId);
    bool IsAdminUser(Guid userProfileId);
    IEnumerable<AdminUser> GetAdmins() ;
}
