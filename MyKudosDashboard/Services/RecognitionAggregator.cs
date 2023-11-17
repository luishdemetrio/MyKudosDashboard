using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.Interfaces.Aggregator;

namespace MyKudosDashboard.Services;

public class RecognitionAggregator : IRecognitionAggregator
{

    private readonly string _gatewayServiceUrl;
    private IRestClientHelper _restClientHelper;

    private readonly ILogger<RecognitionAggregator> _logger;

    public RecognitionAggregator(IConfiguration config, ILogger<RecognitionAggregator> log,
                                      IRestClientHelper clientHelper)
    {
        _gatewayServiceUrl = config["GatewayServiceUrl"];
        _logger = log;
        _restClientHelper = clientHelper;
    }

    public async Task<bool> AddNewRecognition(Recognition group)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<Recognition, bool>(
                            $"{_gatewayServiceUrl}recognition", HttpMethod.Post,
                            group);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing AddNewRecognition: {ex.Message}");
        }

        return result;
    }

    public async Task<bool> DeleteRecognition(int recognitionId)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<int, bool>(
                            $"{_gatewayServiceUrl}recognition/{recognitionId}", HttpMethod.Delete,
                            recognitionId);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing DeleteRecognition: {ex.Message}");
        }

        return result;
    }

    public async Task<IEnumerable<Recognition>> GetRecognitions()
    {
        List<Recognition> result = new();

        try
        {
            var recognitions = await _restClientHelper.GetApiData<IEnumerable<Recognition>>(
                                                    $"{_gatewayServiceUrl}recognition");
            result = recognitions.ToList();
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetRecognitionGroupsAsync: {ex.Message}");
        }

        return result;
    }

    public async Task<bool> UpdateRecognition(Recognition recognition)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<Recognition, bool>(
                            $"{_gatewayServiceUrl}recognition", HttpMethod.Put,
                            recognition);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing UpdateRecognition: {ex.Message}");
        }

        return result;
    }
}
