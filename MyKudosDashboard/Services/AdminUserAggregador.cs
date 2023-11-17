using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.Interfaces.Aggregator;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Services;

public class AdminUserAggregador : IAdminUserAggregador
{
    private readonly string _serviceUrl;
    private IRestClientHelper _restClientHelper;
    private readonly ILogger<AdminUserAggregador> _logger;


    public AdminUserAggregador(IConfiguration config, IRestClientHelper clientHelper,
                            ILogger<AdminUserAggregador> log)
    {
        _serviceUrl = config["GatewayServiceUrl"];
        _logger = log;
        _restClientHelper = clientHelper;
    }

    public async Task<bool> Add(Guid userProfileId)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<Guid, bool>(
                                $"{_serviceUrl}adminuser",
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
                                $"{_serviceUrl}adminuser",
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

    public async Task<IEnumerable<Person>> GetAdminsUsers()
    {
        IEnumerable<Person> result = null;

        try
        {
            result = await _restClientHelper.GetApiData<IEnumerable<Person>>(
                                $"{_serviceUrl}adminuser/getadmins");

        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing Add: {ex.Message}");
        }

        return result;
    }
}
