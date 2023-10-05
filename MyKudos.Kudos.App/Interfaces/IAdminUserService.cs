

namespace MyKudos.Kudos.App.Interfaces;

public interface IAdminUserService
{
    bool Add(Guid userProfileId);
    bool Delete(Guid userProfileId);
    bool IsAdminUser(Guid userProfileId);
}
