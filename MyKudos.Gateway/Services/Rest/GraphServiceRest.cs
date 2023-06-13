using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gateway.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Gateway.Services;

public class GraphServiceRest : IGraphService
{
       
    private  string _graphServiceUrl;

    private IRestClientHelper _restClientHelper;

    private readonly ILogger<GraphServiceRest> _logger;

    public GraphServiceRest(IConfiguration configuration, ILogger<GraphServiceRest> log, IRestClientHelper clientHelper)
    {   
        _graphServiceUrl = configuration["graphServiceUrl"];
        _logger = log;
        _restClientHelper = clientHelper;

    }

    public async Task<List<GraphUser>> GetUsers(string name)
    {
        List<GraphUser> result = new();

        try
        {
            result = await _restClientHelper.GetApiData<List<GraphUser>>($"{_graphServiceUrl}user/?name={name}");

        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetUsers: {ex.Message}");
        }

        return result;

    }


    public async Task<IEnumerable<GraphUserPhoto>> GetUserPhotos(Guid[] usersId)
    {
        List<GraphUserPhoto> result = new();

        try
        {
            var photos = await _restClientHelper.GetApiData<Guid[],IEnumerable<GraphUserPhoto>>($"{_graphServiceUrl}photos", usersId);
            result = photos.ToList();

        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetUserPhotos: {ex.Message}");
        }

        return result;
    }

    public async Task<string> GetUserPhoto(Guid userid)
    {
        string result = string.Empty;

        try
        {
            result = await _restClientHelper.GetApiData<string>($"{_graphServiceUrl}photo/?userid={userid}");            

        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetUserPhoto: {ex.Message}");
        }

        return result;

    }

    public async Task<List<GraphUser>> GetUserInfo(Guid[] users)
    {

        var result = new List<GraphUser>();

        try
        {
            result = await _restClientHelper.GetApiData<Guid[],List<GraphUser>>($"{_graphServiceUrl}userinfo",users);

        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetUserInfo: {ex.Message}");
        }

        return result;

       
    }

    public async Task<Guid> GetUserManagerAsync(Guid userid)
    {
        var result = Guid.Empty;

        try
        {
            result = await _restClientHelper.GetApiData<Guid>($"{_graphServiceUrl}manager/?userid={userid}");

        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetUserManagerAsync: {ex.Message}");
        }

        return result;

    }
}