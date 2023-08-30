using Microsoft.AspNetCore.Mvc;
using SuperKudos.Aggregator.Interfaces;
using SuperKudos.Aggregator.Domain.Models;
using GatewayDomain = SuperKudos.Aggregator.Domain.Models;
using SuperKudos.Aggregator.Helpers;

namespace SuperKudos.Aggregator.Controllers;

[ApiController]
[Route("[controller]")]
public class KudosFromMeController : Controller
{
    
    private readonly IGraphService _graphService;
    private readonly IRecognitionService _recognitionService;
    private readonly IKudosService _kudosService;

    private string _defaultProfilePicture;

    private IEnumerable<GatewayDomain.Recognition> _recognitions;

    
    public KudosFromMeController(IGraphService graphService, IRecognitionService recognitionService, 
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


    [HttpGet(Name = "GetKudosFromMe")]
    public async Task<IEnumerable<KudosResponse>> Get(string userId, int pageNumber = 1)
    {
        //get kudos
        var kudos = await _kudosService.GetKudosFromMeAsync(userId, pageNumber);

        return KudosHelper.GetKudos(kudos, _defaultProfilePicture);

    }

   
}
