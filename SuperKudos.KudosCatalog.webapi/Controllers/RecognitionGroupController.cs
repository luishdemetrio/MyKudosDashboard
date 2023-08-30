using Microsoft.AspNetCore.Mvc;
using SuperKudos.KudosCatalog.App.Interfaces;
using SuperKudos.KudosCatalog.Domain.Models;

namespace SuperKudos.KudosCatalog.webapi.Controllers;

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
