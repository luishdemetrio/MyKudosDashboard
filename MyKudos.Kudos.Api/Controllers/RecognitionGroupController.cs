using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RecognitionGroupController : Controller
{

    private readonly IRecognitionGroupRepository _recognitionRepository;


    public RecognitionGroupController(IRecognitionGroupRepository recognitionRepository)
    {
        _recognitionRepository = recognitionRepository;
    }

    [HttpGet(Name = "GetRecognitionsGroup")]
    public IEnumerable<Domain.Models.RecognitionGroup> Get()
    {
        return _recognitionRepository.GetRecognitionsGroup();
    }

    [HttpPost(Name = "SetRecognitionGroup")]
    public bool SetRecognitionGroup([FromBody] RecognitionGroup recognitionGroup)
    {
        return _recognitionRepository.AddNewRecognitionGroup(recognitionGroup);

    }

    // PUT: api/RecognitionsGroup/5
    [HttpPut()]
    public async Task<IActionResult> UpdateRecognitionsGroup([FromBody] RecognitionGroup group)
    {
        
        if (_recognitionRepository.UpdateRecognitionGroup(group))
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

        return _recognitionRepository.DeleteRecognitionGroup(id);
    }
}
