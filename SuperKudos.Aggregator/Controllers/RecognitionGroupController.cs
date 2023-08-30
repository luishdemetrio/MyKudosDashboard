using Microsoft.AspNetCore.Mvc;
using SuperKudos.Aggregator.Domain.Models;
using SuperKudos.Aggregator.Interfaces;

namespace SuperKudos.Aggregator.Controllers;

[ApiController]
[Route("[controller]")]
public class RecognitionGroupController : Controller
{

    private readonly IRecognitionGroupService _recognitionGroupService;


    public RecognitionGroupController(IRecognitionGroupService recognitionGroupService)
    {
        _recognitionGroupService = recognitionGroupService;
    }

    [HttpGet(Name = "GetRecognitionsGroup")]
    public Task<IEnumerable<RecognitionGroup>> Get()
    {
        return _recognitionGroupService.GetRecognitionGroups();
    }

    [HttpPost(Name = "SetRecognitionGroup")]
    public bool SetRecognitionGroup([FromBody] RecognitionGroup recognitionGroup)
    {
        return _recognitionGroupService.SetRecognitionGroups(recognitionGroup);

    }
}
