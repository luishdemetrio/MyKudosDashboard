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
    public async Task<bool> SendLikeAsync([FromBody] LikeGateway like)
    {
        return await ProcessLikeAsync(like, _kudosService.LikeAsync);
    }

    [HttpDelete(Name = "Undolike")]
    public async Task<bool> Delete([FromBody] LikeGateway unlike)
    {
        return await ProcessLikeAsync(unlike, _kudosService.UndoLikeAsync);
    }


    private async Task<bool> ProcessLikeAsync([FromBody] LikeGateway like, Func<Kudos.Domain.Models.SendLike, Task<bool>> likeAction)
    {
        bool result = false;

        result = await likeAction(new Kudos.Domain.Models.SendLike
        (
            KudosId: like.KudosId,
            FromPersonId: like.FromPerson.Id
        ));

        //we need to get who is receiving the kudos to update their score on the dashboard 
        var kudos = await _kudosService.GetKudosUser(like.KudosId);

        if (kudos != null)
        {
            await _kudosQueue.SendLikeAsync(like, kudos.Recognized);
        }

        return result;
    }

}
