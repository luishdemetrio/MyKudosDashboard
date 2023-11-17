using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gateway.Domain.Models;
using MyKudos.Gateway.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Gateway.Services;

public class RecognitionServiceRest : IRecognitionService
{

    private readonly string _recognitionServiceUrl;
    private IRestClientHelper _restClientHelper;

    private readonly ILogger<RecognitionServiceRest> _logger;

    public RecognitionServiceRest(IConfiguration config, ILogger<RecognitionServiceRest> log, IRestClientHelper clientHelper)
    {
        _recognitionServiceUrl = config["KudosServiceUrl"];

        _logger = log;
        _restClientHelper = clientHelper;
    }

    public async Task<IEnumerable<Domain.Models.Recognition>> GetRecognitionsAsync()
    {
        var result = new List<Domain.Models.Recognition>();

        try
        {
            var recognitions = await _restClientHelper.GetApiData<IEnumerable<Domain.Models.Recognition>>($"{_recognitionServiceUrl}recognition");
            result = recognitions.ToList();
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetRecognitionsAsync: {ex.Message}");
        }

        return result;


    }

    public async Task<bool> AddRecognition(Domain.Models.Recognition recognition)
    {
        var result = false;

        try
        {
            result = await _restClientHelper.SendApiData<Domain.Models.Recognition, bool>(
                                    $"{_recognitionServiceUrl}recognition",
                                    HttpMethod.Post
                                    , recognition);

        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing AddNewRecognitionGroup: {ex.Message}");
        }

        return result;
    }

    public async Task<bool> DeleteRecognition(int recognitionId)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<int, bool>(
                                    $"{_recognitionServiceUrl}recognition/{recognitionId}",
                                    HttpMethod.Delete
                                    , recognitionId);

        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing AddNewRecognitionGroup: {ex.Message}");
        }

        return result;
    }

    public async Task<bool> UpdateRecognition(Domain.Models.Recognition recognition)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<Domain.Models.Recognition, bool>(
                                    $"{_recognitionServiceUrl}recognition/{recognition.RecognitionId}",
                                    HttpMethod.Put,
                                    recognition);

        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing AddNewRecognitionGroup: {ex.Message}");
        }

        return result;
    }
}
