using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Api.Controllers;

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