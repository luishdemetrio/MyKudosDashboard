using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gateway.Interfaces;
using MyKudos.Kudos.Domain.Models;


namespace MyKudos.Gateway.Services;

public class KudosServiceRest: IKudosService
{

    private readonly string _kudosServiceUrl;

    private IRestClientHelper _restClientHelper;

    private readonly ILogger<KudosServiceRest> _logger;

    public KudosServiceRest(IConfiguration config, ILogger<KudosServiceRest> log, IRestClientHelper clientHelper)
    {
        _kudosServiceUrl = config["kudosServiceUrl"];
        _logger = log;
        _restClientHelper = clientHelper;
    }

    public async Task<IEnumerable<Kudos.Domain.Models.Kudos>> GetKudosAsync(int pageNumber, 
                                                                            Guid? managerId = null, 
                                                                            int? sentOnYear = null)
    {
        return await GetKudosDataAsync($"kudos/?pageNumber={pageNumber}&managerId={managerId}&sentOnYear={sentOnYear}");
    }

    public async Task<IEnumerable<Kudos.Domain.Models.Kudos>> GetKudosFromMeAsync(string userId, int pageNumber, 
                                                                                  Guid? managerId = null, 
                                                                                  int? sentOnYear = null)
    {
        return await GetKudosDataAsync($"kudosfromme/?userid={userId}&pageNumber={pageNumber}&managerId={managerId}&sentOnYear={sentOnYear}");
    }

    public async Task<IEnumerable<Kudos.Domain.Models.Kudos>> GetKudosToMeAsync(string userId, int pageNumber, Guid? managerId = null, 
                                                                                int? sentOnYear = null)
    {
        return await GetKudosDataAsync($"kudostome/?userid={userId}&pageNumber={pageNumber}&managerId={managerId}&sentOnYear={sentOnYear}");
    }

    private async Task<IEnumerable<Kudos.Domain.Models.Kudos>> GetKudosDataAsync(string endpoint)
    {
        List<Kudos.Domain.Models.Kudos> result = new();

        try
        {
            var kudos = await _restClientHelper.GetApiData<IEnumerable<Kudos.Domain.Models.Kudos>>(
                $"{_kudosServiceUrl}{endpoint}");
            result = kudos.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing {nameof(GetKudosDataAsync)}: {ex.Message}");
        }

        return result;
    }


    public async Task<int> SendAsync(Kudos.Domain.Models.Kudos kudos)
    {
        var result = 0;

        try
        {
            result = await _restClientHelper.SendApiData<Kudos.Domain.Models.Kudos, int>($"{_kudosServiceUrl}kudos", HttpMethod.Post, kudos);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing SendKudos: {ex.Message}");
        }

        return result;

    }

    public async Task<bool> LikeAsync(SendLike like)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<SendLike, bool>($"{_kudosServiceUrl}like", HttpMethod.Post, like);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing Like: {ex.Message}");
        }

        return result;

    }

    public async Task<bool> UndoLikeAsync(SendLike like)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<SendLike, bool>($"{_kudosServiceUrl}like", HttpMethod.Delete, like);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing UndoLike: {ex.Message}");
        }

        return result;

    }

   
    public async Task<Kudos.Domain.Models.Kudos> GetKudosUser(int kudosId)
    {
        Kudos.Domain.Models.Kudos result = new();

        try
        {
            result = await _restClientHelper.GetApiData<Kudos.Domain.Models.Kudos>($"{_kudosServiceUrl}kudosid/?kudosid= {kudosId}");
            
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing GetKudos: {ex.Message}");
        }

        return result;
    }

    public async Task<IEnumerable<Kudos.Domain.Models.Kudos>> GetKudosToMyDirectsAsync(string userId, int pageNumber)
    {
        List<Kudos.Domain.Models.Kudos> result = new();

        try
        {
            var kudos = await _restClientHelper.GetApiData<IEnumerable<Kudos.Domain.Models.Kudos>>($"{_kudosServiceUrl}kudostomydirects/?userid={userId}&pageNumber= {pageNumber}");
            result = kudos.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing GetKudos: {ex.Message}");
        }

        return result;
    }

    public async Task<bool> UpdateKudos(Kudos.Domain.Models.KudosMessage kudos)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<Kudos.Domain.Models.KudosMessage, bool>($"{_kudosServiceUrl}kudos", HttpMethod.Put, kudos);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing UndoLike: {ex.Message}");
        }

        return result;
    }

    public async Task<bool> DeleteKudos(int kudosId)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<int, bool>($"{_kudosServiceUrl}kudos", HttpMethod.Delete, kudosId);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing UndoLike: {ex.Message}");
        }

        return result;
    }
}
