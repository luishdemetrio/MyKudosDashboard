using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gateway.Interfaces;

namespace MyKudos.Gateway.Services;

public class RecognitionServiceRest : IRecognitionService
{

    private readonly string _recognitionServiceUrl;
    private IRestClientHelper _restClientHelper;

    private readonly ILogger<RecognitionServiceRest> _logger;

    public RecognitionServiceRest(IConfiguration config, ILogger<RecognitionServiceRest> log, IRestClientHelper clientHelper)
    {
        _recognitionServiceUrl = config["RecognitionServiceUrl"];

        _logger = log;
        _restClientHelper = clientHelper;
    }

    public async Task<IEnumerable<Models.Recognition>> GetRecognitionsAsync()
    {
        var result = new List<Models.Recognition>();

        try
        {
            var recognitions = await _restClientHelper.GetApiData<IEnumerable<Models.Recognition>>($"{_recognitionServiceUrl}recognition");
            result = recognitions.ToList();
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetRecognitionsAsync: {ex.Message}");
        }

        return result;


    }
}
