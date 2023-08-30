using MyKudos.Communication.Helper.Interfaces;
using SuperKudos.Aggregator.Domain.Models;
using SuperKudos.Aggregator.Interfaces;

namespace SuperKudos.Aggregator.Services;

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


    public bool SetRecognitionGroups(RecognitionGroup group)
    {
        throw new NotImplementedException();
    }
}
