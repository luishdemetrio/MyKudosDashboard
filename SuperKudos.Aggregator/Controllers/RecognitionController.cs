using Microsoft.AspNetCore.Mvc;
using SuperKudos.Aggregator.Interfaces;
using SuperKudos.Aggregator.Domain.Models;

namespace SuperKudos.Aggregator.Controllers;


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
    public async Task<IEnumerable<Recognition>> GetAsync()
    {
        return await _recognitionService.GetRecognitionsAsync();

    }



}