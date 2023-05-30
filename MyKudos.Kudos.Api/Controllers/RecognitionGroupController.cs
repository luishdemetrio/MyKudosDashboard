using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Api.Controllers;

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
    public IEnumerable<Domain.Models.RecognitionGroup> Get()
    {
        return _recognitionGroupService.GetRecognitionGroups();
    }

    [HttpPost(Name = "SetRecognitionGroup")]
    public bool SetRecognitionGroup([FromBody] RecognitionGroup recognitionGroup)
    {
        return _recognitionGroupService.SetRecognitionGroups(recognitionGroup);

    }
}
