using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Domain.Models;
using MyKudos.Kudos.Domain.Models;
using GatewayDomain = MyKudos.Gateway.Domain.Models;
using Microsoft.Graph;
using MyKudos.Gateway.Helpers;

namespace MyKudos.Gateway.Controllers;

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
    public async Task<IEnumerable<KudosResponse>> Get(string userId, int pageNumber = 1, int? sentOnYear = null)
    {
        //get kudos
        var kudos = await _kudosService.GetKudosToMeAsync(userId, pageNumber, null, sentOnYear);

        return KudosHelper.GetKudos(kudos, _defaultProfilePicture);

    }


}
