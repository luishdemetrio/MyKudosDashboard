using Microsoft.AspNetCore.Mvc;
using MyKudos.Dashboard.App.Interface;
using MyKudos.Dashboard.Domain.Models;


namespace MyKudos.Dashboard.Api.Controllers;

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

}