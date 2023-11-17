using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gateway.Domain.Models;
using MyKudos.Gateway.Interfaces;

namespace MyKudos.Gateway.Services;

public class RecognitionGroupServiceRest : IRecognitionGroupService
{

    private readonly string _recognitionServiceUrl;
    private IRestClientHelper _restClientHelper;

    private readonly ILogger<RecognitionServiceRest> _logger;

    public RecognitionGroupServiceRest(IConfiguration config, ILogger<RecognitionServiceRest> log, IRestClientHelper clientHelper)
    {
        _recognitionServiceUrl = config["KudosServiceUrl"];

        _logger = log;
        _restClientHelper = clientHelper;
    }

    public async Task<IEnumerable<RecognitionGroup>> GetRecognitionGroups()
    {
        var result = new List<RecognitionGroup>();

        try
        {
            var recognitions = await _restClientHelper.GetApiData<IEnumerable<RecognitionGroup>>($"{_recognitionServiceUrl}recognitiongroup");
            result = recognitions.ToList();
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetRecognitionsAsync: {ex.Message}");
        }

        return result;


    }


    public async Task<bool> AddNewRecognitionGroup(RecognitionGroup group)
    {
        var result = false;

        try
        {
            result = await _restClientHelper.SendApiData< RecognitionGroup, bool>(
                                    $"{_recognitionServiceUrl}recognitiongroup",
                                    HttpMethod.Post
                                    ,group);
           
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing AddNewRecognitionGroup: {ex.Message}");
        }

        return result;
    }

    public async Task<bool> DeleteRecognitionGroup(int recognitionGroupId)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<int, bool>(
                                    $"{_recognitionServiceUrl}recognitiongroup/{recognitionGroupId}",
                                    HttpMethod.Delete
                                    , recognitionGroupId);

        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing AddNewRecognitionGroup: {ex.Message}");
        }

        return result;
    }

    public async Task<bool> UpdateRecognitionGroup(RecognitionGroup group)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<RecognitionGroup, bool>(
                                    $"{_recognitionServiceUrl}recognitiongroup",
                                    HttpMethod.Put,
                                    group);

        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing AddNewRecognitionGroup: {ex.Message}");
        }

        return result;
    }
}
