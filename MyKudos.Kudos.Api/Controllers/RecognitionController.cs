using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RecognitionController : ControllerBase
{

    private readonly IRecognitionRepository _repository;


    public RecognitionController(IRecognitionRepository recognitionService)
    {
        _repository = recognitionService;
    }

    [HttpGet(Name = "GetRecognitions")]
    public IEnumerable<Domain.Models.Recognition> Get()
    {
        return _repository.GetRecognitions();
    }

    [HttpPost(Name = "SetRecognition")]
    public bool SetRecognition([FromBody] Recognition recognition)
    {
        return _repository.SetRecognition(recognition);

    }

    // PUT: api/RecognitionsGroup/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRecognition(Recognition recognition)
    {
       
        if (_repository.UpdateRecognition(recognition))
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
    public bool DeleteRecognition(int id)
    {

        return _repository.DeleteRecognition(id);
    }
}