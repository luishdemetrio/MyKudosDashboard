using MyKudos.Communication.Helper.Interfaces;
using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Services;

public class UserGateway : IUserGateway
{


    private readonly string _gatewayServiceUrl;
    private IRestClientHelper _restClientHelper;

    private readonly ILogger<UserGateway> _logger;

    public UserGateway(IConfiguration config, ILogger<UserGateway> log, IRestClientHelper clientHelper)
    {
        _gatewayServiceUrl = config["GatewayServiceUrl"];
        _logger = log;
        _restClientHelper = clientHelper;
    }

    public async Task<IEnumerable<UserViewModel>> GetUsers(string name)
    {
        List<UserViewModel> result = new();

        try
        {
            var users = await _restClientHelper.GetApiData<IEnumerable<UserViewModel>>($"{_gatewayServiceUrl}user/?name={name}");
            result = users.ToList();
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetUsers: {ex.Message}");
        }

        return result;

    }

    public async Task<string> GetUserPhoto(string userid)
    {
        string result = string.Empty;

        try
        {
            result =  await _restClientHelper.GetApiData<string>($"{_gatewayServiceUrl}photo/?userid={userid}");
            
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetUserPhoto: {ex.Message}");
        }

        return result;

    }
}
