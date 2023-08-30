using Microsoft.AspNetCore.Mvc;
using SuperKudos.KudosCatalog.App.Interfaces;
using SuperKudos.KudosCatalog.Domain.Models;

namespace SuperKudos.KudosCatalog.webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class RecognitionController : ControllerBase
{

    private readonly IRecognitionService _recognitionService;


    public RecognitionController(IRecognitionService recognitionService)
    {
        _recognitionService = recognitionService;
    }

    [HttpGet(Name = "GetRecognitions")]
    public IEnumerable<Domain.Models.Recognition> Get()
    {
        return _recognitionService.GetRecognitions();
    }

    [HttpPost(Name = "SetRecognition")]
    public bool SetRecognition([FromBody] Recognition recognition)
    {
        return _recognitionService.SetRecognition(recognition);

    }
}