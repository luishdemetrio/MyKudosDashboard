using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Domain.Models;

namespace MyKudos.Gateway.Controllers;


[ApiController]
[Route("[controller]")]
public class RecognitionController : ControllerBase
{

    private readonly IRecognitionService _recognitionService;
    private readonly IRecognitionGroupService _recognitionGroupService;

    public RecognitionController( IRecognitionService recognitionService,
                                  IRecognitionGroupService recognitionGroupService)
    {
        _recognitionService = recognitionService;   
        _recognitionGroupService = recognitionGroupService;
    }

    [HttpGet(Name = "GetRecognitions")]
    public async Task<IEnumerable<Recognition>> GetAsync()
    {
        var groups = await _recognitionGroupService.GetRecognitionGroups();

        var recognitions = await _recognitionService.GetRecognitionsAsync();

        var result = from r in recognitions
                     join g in groups on r.RecognitionGroupId equals g.RecognitionGroupId
                     select new Recognition
                     {
                         RecognitionId = r.RecognitionId,
                         RecognitionGroupId = r.RecognitionGroupId,
                         Title = r.Title,
                         Description = r.Description,
                         DisplayOrder = r.DisplayOrder,
                         RecognitionGroupName = g.Description
                     };

        return result.OrderBy(r => r.DisplayOrder); 
    }

    [HttpPost(Name = "AddRecognition")]
    public async Task<bool> SetRecognition([FromBody] Recognition recognition)
    {
        return await _recognitionService.AddRecognition(recognition);

    }

    // PUT: api/Recognition/5
    [HttpPut()]
    public async Task<IActionResult> UpdateRecognition([FromBody] Recognition recognition)
    {

       
        if (await _recognitionService.UpdateRecognition(recognition))
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
    public async Task<bool> DeleteRecognition(int id)
    {

        return await _recognitionService.DeleteRecognition(id);
    }


}