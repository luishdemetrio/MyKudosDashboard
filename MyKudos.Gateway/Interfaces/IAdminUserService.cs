namespace MyKudos.Gateway.Interfaces;

public interface IAdminUserService
{
    Task<bool> Add(Guid userProfileId);
    Task<bool> Delete(Guid userProfileId);
    Task<bool> IsAdminUser(Guid userProfileId);
}