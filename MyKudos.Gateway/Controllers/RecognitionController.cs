using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Interfaces;

namespace MyKudos.Gateway.Controllers;


[ApiController]
[Route("[controller]")]
public class RecognitionController : ControllerBase
{

    private readonly IRecognitionService _recognitionService;

    public RecognitionController( IRecognitionService recognitionService)
    {
        _recognitionService = recognitionService;   
    }

    [HttpGet(Name = "GetRecognitions")]
    public IEnumerable<Models.Recognition> Get()
    {
        return _recognitionService.GetRecognitions();

    }



}