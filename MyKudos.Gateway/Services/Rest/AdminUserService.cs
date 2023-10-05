using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gateway.Interfaces;

namespace MyKudos.Gateway.Services.Rest;

public class AdminUserService : IAdminUserService
{
    private readonly string _serviceUrl;
    private IRestClientHelper _restClientHelper;

    private readonly ILogger<AdminUserService> _logger;

    public AdminUserService(IConfiguration config, IRestClientHelper clientHelper,
                            ILogger<AdminUserService> log)
    {
        _serviceUrl = config["kudosServiceUrl"];
        _logger = log;
        _restClientHelper = clientHelper;
    }

    public async Task<bool> Add(Guid userProfileId)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<Guid, bool>(
                                $"{_serviceUrl}kudos", 
                                HttpMethod.Post, 
                                userProfileId);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing Add: {ex.Message}");
        }

        return result;
    }

    public async Task<bool> Delete(Guid userProfileId)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<Guid, bool>(
                                $"{_serviceUrl}kudos",
                                HttpMethod.Delete,
                                userProfileId);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing Delete: {ex.Message}");
        }

        return result;
    }

    public async Task<bool> IsAdminUser(Guid userProfileId)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.GetApiData<bool>(
                                $"{_serviceUrl}adminuser/?userprofileid={userProfileId}");

        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing Add: {ex.Message}");
        }

        return result;
    }
}
