using MyKudos.Communication.Helper.Interfaces;
using SuperKudos.Aggregator.Interfaces;
using SuperKudos.KudosCatalog.Domain.Models;


namespace SuperKudos.Aggregator.Services;

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

    public async Task<IEnumerable<Kudos>> GetKudosAsync(int pageNumber)
    {
        List<Kudos> result = new();

        try
        {
            var kudos = await _restClientHelper.GetApiData<IEnumerable<Kudos>>($"{_kudosServiceUrl}kudos/?pageNumber= {pageNumber}");
            result = kudos.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing GetKudos: {ex.Message}");
        }

        return result;

    }

    public async Task<int> SendAsync(Kudos kudos)
    {
        var result = 0;

        try
        {
            result = await _restClientHelper.SendApiData<Kudos, int>($"{_kudosServiceUrl}kudos", HttpMethod.Post, kudos);
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

    public async Task<IEnumerable<Kudos>> GetKudosFromMeAsync(string userId, int pageNumber)
    {
        List<Kudos> result = new();

        try
        {
            var kudos = await _restClientHelper.GetApiData<IEnumerable<Kudos>>($"{_kudosServiceUrl}kudosfromme/?userid={userId}&pageNumber= {pageNumber}");
            result = kudos.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing GetKudos: {ex.Message}");
        }

        return result;
    }

    public async Task<IEnumerable<Kudos>> GetKudosToMeAsync(string userId, int pageNumber)
    {
        List<Kudos> result = new();

        try
        {
            var kudos = await _restClientHelper.GetApiData<IEnumerable<Kudos>>($"{_kudosServiceUrl}kudostome/?userid={userId}&pageNumber= {pageNumber}");
            result = kudos.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing GetKudos: {ex.Message}");
        }

        return result;
    }

    public async Task<Kudos> GetKudosUser(int kudosId)
    {
        Kudos result = new();

        try
        {
            result = await _restClientHelper.GetApiData<Kudos>($"{_kudosServiceUrl}kudosid/?kudosid= {kudosId}");
            
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing GetKudos: {ex.Message}");
        }

        return result;
    }

    
}
