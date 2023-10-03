using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.Interfaces.Aggregator;

namespace MyKudosDashboard.Services;

public class RecognitionGroupAggregator : IRecognitionGroupAggregator
{

    private readonly string _gatewayServiceUrl;
    private IRestClientHelper _restClientHelper;

    private readonly ILogger<RecognitionGroupAggregator> _logger;

    public RecognitionGroupAggregator(IConfiguration config, ILogger<RecognitionGroupAggregator> log, 
                                      IRestClientHelper clientHelper)
    {
        _gatewayServiceUrl = config["GatewayServiceUrl"];
        _logger = log;
        _restClientHelper = clientHelper;
    }

    public async Task<bool> AddNewRecognitionGroup(RecognitionGroup group)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<RecognitionGroup, bool>(
                            $"{_gatewayServiceUrl}recognitiongroup", HttpMethod.Post,
                            group);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing SendKudos: {ex.Message}");
        }

        return result;
    }

    public async Task<bool> DeleteRecognitionGroup(int recognitionGroupId)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<int, bool>(
                            $"{_gatewayServiceUrl}recognitiongroup", HttpMethod.Delete,
                            recognitionGroupId);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing SendKudos: {ex.Message}");
        }

        return result;
    }

    public async Task<IEnumerable<RecognitionGroup>> GetRecognitionGroups()
    {
        List<RecognitionGroup> result = new();

        try
        {
            var recognitions = await _restClientHelper.GetApiData<IEnumerable<RecognitionGroup>>(
                                                    $"{_gatewayServiceUrl}recognitiongroup");
            result = recognitions.ToList();
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetRecognitionGroupsAsync: {ex.Message}");
        }

        return result;
    }

    public async Task<bool> UpdateRecognitionGroup(RecognitionGroup group)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<RecognitionGroup, bool>(
                            $"{_gatewayServiceUrl}recognitiongroup", HttpMethod.Put,
                            group);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing SendKudos: {ex.Message}");
        }

        return result;
    }
}
