using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Interfaces;

namespace MyKudos.Kudos.App.Services;

public sealed class AdminUserService : IAdminUserService
{
    private IAdminUserRepository _repository;

    public AdminUserService(IAdminUserRepository repository)
    {
        _repository = repository;
    }

    public bool Add(Guid userProfileId)
    {
        return _repository.Add(userProfileId);
    }

    public bool Delete(Guid userProfileId)
    {
        return _repository.Delete(userProfileId);
    }

    public bool IsAdminUser(Guid userProfileId)
    {
        return _repository.IsAdminUser(userProfileId);
    }
}
