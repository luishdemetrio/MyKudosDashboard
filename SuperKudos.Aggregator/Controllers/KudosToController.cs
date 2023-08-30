using Microsoft.AspNetCore.Mvc;
using SuperKudos.Aggregator.Interfaces;
using SuperKudos.Aggregator.Domain.Models;
using SuperKudos.KudosCatalog.Domain.Models;
using GatewayDomain = SuperKudos.Aggregator.Domain.Models;
using Microsoft.Graph;
using SuperKudos.Aggregator.Helpers;

namespace SuperKudos.Aggregator.Controllers;

[ApiController]
[Route("[controller]")]
public class KudosToMeController : Controller
{
    
    private readonly IGraphService _graphService;
    private readonly IRecognitionService _recognitionService;
    private readonly IKudosService _kudosService;
   

    private IEnumerable<GatewayDomain.Recognition> _recognitions;

    private string _defaultProfilePicture;

    public KudosToMeController(IGraphService graphService, IRecognitionService recognitionService, 
                               IKudosService kudosService, IConfiguration configuration)
    {
        
        _graphService = graphService;
        _recognitionService = recognitionService;
        _kudosService = kudosService;

        _ = PopulateRecognitionsAsync();

        _defaultProfilePicture = configuration["DefaultProfilePicture"];

    }

    private async Task PopulateRecognitionsAsync()
    {
        _recognitions = await _recognitionService.GetRecognitionsAsync().ConfigureAwait(false);
    }


    [HttpGet(Name = "GetKudosToMe")]
    public async Task<IEnumerable<KudosResponse>> Get(string userId, int pageNumber = 1)
    {
        //get kudos
        var kudos = await _kudosService.GetKudosToMeAsync(userId, pageNumber);

        return KudosHelper.GetKudos(kudos, _defaultProfilePicture);

    }


}
