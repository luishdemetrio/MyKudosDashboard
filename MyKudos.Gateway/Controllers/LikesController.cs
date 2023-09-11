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
    public async Task<bool> SendLikeAsync([FromBody] SendLikeGateway like)
    {
        return await ProcessLikeAsync(like, _kudosService.LikeAsync);
    }

    [HttpDelete(Name = "Undolike")]
    public async Task<bool> Delete([FromBody] SendLikeGateway unlike)
    {
        return await ProcessLikeAsync(unlike, _kudosService.UndoLikeAsync);
    }


    private async Task<bool> ProcessLikeAsync([FromBody] SendLikeGateway like, Func<Kudos.Domain.Models.SendLike, Task<bool>> likeAction)
    {
        bool result = false;

        result = await likeAction(new Kudos.Domain.Models.SendLike
        (
            KudosId: like.KudosId,
            FromPersonId: like.UserProfileId
        ));

        //we need to get who is receiving the kudos to update their score on the dashboard 
        var kudos = await _kudosService.GetKudosUser(like.KudosId);

        if (kudos != null)
        {
            var whoLiked = kudos.Likes.Where(p=> p.PersonId == like.UserProfileId).First();

            if (whoLiked != null)
            {
                await _kudosQueue.SendLikeAsync(
                    new LikeGateway(
                        KudosId: whoLiked.KudosId,
                        FromPerson: new Person()
                        {
                            Id = whoLiked.Person.UserProfileId,
                            Name = whoLiked.Person.DisplayName,
                            GivenName = whoLiked.Person.GivenName,
                            Photo = whoLiked.Person.Photo
                        }
                    ),
                    kudos.Recognized);
            }
            
        }

        return result;
    }

}
