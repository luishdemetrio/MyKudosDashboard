using MyKudos.Communication.Helper.Interfaces;
using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;

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

    public async Task<IEnumerable<RecognitionViewModel>> GetRecognitionsAsync()
    {

        List<RecognitionViewModel> result = new();

        try
        {
            var recognitions = await _restClientHelper.GetApiData<IEnumerable<RecognitionViewModel>>($"{_gatewayServiceUrl}recognition");
            result = recognitions.ToList();
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetRecognitionsAsync: {ex.Message}");
        }

        return result;


    }
}
