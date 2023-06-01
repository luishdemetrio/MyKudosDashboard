using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Domain.Models;

namespace MyKudos.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class LikesController : Controller
{

    private readonly IKudosService _kudosService;

    private IKudosMessageSender _kudosQueue;

    public LikesController(IKudosService kudosService, IKudosMessageSender kudosQueue)
    {

        _kudosService = kudosService;

        _kudosQueue = kudosQueue;
    }

    [HttpPost(Name = "SendLike")]
    public  async Task<bool> SendLikeAsync([FromBody] LikeGateway like)
    {
        bool result = false;
        
        result = await _kudosService.LikeAsync(new Kudos.Domain.Models.SendLike
        (
            KudosId : like.KudosId,
            FromPersonId : like.FromPerson.Id
        ));

        await _kudosQueue.SendLikeAsync(like);

        return result;

    }

    [HttpDelete(Name = "Undolike")]
    public async Task<bool> Delete([FromBody] LikeGateway unlike)
    {
        

        bool result = false;

        result = await _kudosService.UndoLikeAsync(new Kudos.Domain.Models.SendLike
        (
            KudosId: unlike.KudosId,
            FromPersonId: unlike.FromPerson.Id
        ));

        await _kudosQueue.SendUndoLikeAsync(unlike);

        return result;

    }
}
