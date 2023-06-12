using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.Interfaces;

namespace MyKudosDashboard.Services;

public class RecognitionGateway : IRecognitionGateway
{

    private readonly string _gatewayServiceUrl;
    private IRestClientHelper _restClientHelper;

    private readonly ILogger<RecognitionGateway> _logger;

    public RecognitionGateway(IConfiguration config, ILogger<RecognitionGateway> log, IRestClientHelper clientHelper)
    {
        _gatewayServiceUrl = config["GatewayServiceUrl"];
        _logger = log;
        _restClientHelper = clientHelper;
    }

    public async Task<IEnumerable<Recognition>> GetRecognitionsAsync()
    {

        List<Recognition> result = new();

        try
        {
            var recognitions = await _restClientHelper.GetApiData<IEnumerable<Recognition>>($"{_gatewayServiceUrl}recognition");
            result = recognitions.ToList();
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetRecognitionsAsync: {ex.Message}");
        }

        return result;


    }

    public async Task<IEnumerable<RecognitionGroup>> GetRecognitionGroups()
    {
        var result = new List<RecognitionGroup>();

        try
        {
            var recognitions = await _restClientHelper.GetApiData<IEnumerable<RecognitionGroup>>($"{_gatewayServiceUrl}recognitiongroup");
            result = recognitions.ToList();
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetRecognitionsAsync: {ex.Message}");
        }

        return result;
    }
}
