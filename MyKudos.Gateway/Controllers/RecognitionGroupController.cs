using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Domain.Models;
using MyKudos.Gateway.Interfaces;

namespace MyKudos.Gateway.Controllers;

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

    // PUT: api/RecognitionsGroup/5
    [HttpPut()]
    public async Task<IActionResult> PutRecognitionsGroup([FromBody] RecognitionGroup group)
    {
        
        if (await _recognitionGroupService.UpdateRecognitionGroup(group))
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }

    // DELETE: api/RecognitionsGroup/5
    [HttpDelete("{id}")]
    public async Task<bool> DeleteRecognitionsGroup(int id)
    {

        return await _recognitionGroupService.DeleteRecognitionGroup(id);
    }

    [HttpPost(Name = "SetRecognitionGroup")]
    public async Task<bool> SetRecognitionGroup([FromBody] RecognitionGroup recognitionGroup)
    {
        return await _recognitionGroupService.AddNewRecognitionGroup(recognitionGroup);

    }
}
