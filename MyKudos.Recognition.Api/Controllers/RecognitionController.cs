using Microsoft.AspNetCore.Mvc;
using MyKudos.Recognition.App.Interfaces;

namespace MyKudos.Recognition.Api.Controllers;

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

    [HttpPost(Name = "CheckAndSeedDatabaseAsync")]
    public async Task CheckAndSeedDatabaseAsync()
    {
        _recognitionService.SeedDatabase();

    }
}