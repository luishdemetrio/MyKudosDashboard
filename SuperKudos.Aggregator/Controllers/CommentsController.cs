using Microsoft.AspNetCore.Mvc;
using SuperKudos.Aggregator.Interfaces;
using GatewayDomain = SuperKudos.Aggregator.Domain.Models;
using SuperKudos.Aggregator.Domain.Models;
using SuperKudos.KudosCatalog.Domain.Models;


namespace SuperKudos.Aggregator.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : Controller
{


    private readonly IKudosService _kudosService;

    private readonly ICommentsService _commentsService;

    private ICommentsMessageSender _commentsMessageSender;

    private readonly IGraphService _graphService;

    private string _defaultProfilePicture;

    public CommentsController(ICommentsService commentsService, ICommentsMessageSender commentsMessageSender,
                              IGraphService graphService, IKudosService kudosService,
                              IConfiguration configuration)
    {

        _commentsService = commentsService;

        _commentsMessageSender = commentsMessageSender;
        _graphService = graphService;
        _kudosService = kudosService;

        _defaultProfilePicture = configuration["DefaultProfilePicture"];
    }

    [HttpPost(Name = "SendMessage")]
    public async Task<int> Post(CommentsRequest comments)
    {
        int commentId ;

        commentId =  await _commentsService.SendCommentsAsync(new Comments()
        {
            KudosId = comments.KudosId,
            Message = comments.Message,
            FromPersonId = comments.FromPersonId,
            Date = comments.Date
        });

        if (commentId != 0)
        {
            comments.Id = commentId;

            //we need to get who is receiving the kudos to update their score on the dashboard 
            var kudos = await _kudosService.GetKudosUser(comments.KudosId);

            if (kudos != null)
            {
                await _commentsMessageSender.MessageSent(comments, kudos.Recognized);
            }
        }

        return commentId;
    }

    [HttpGet(Name = "GetComments")]
    public async Task<IEnumerable<CommentsResponse>> Get(int kudosId)
    {

        var result = new List<CommentsResponse>();

        var comments =  await _commentsService.GetComments(kudosId);

        if (comments == null)
            return result;

        foreach (var comment in comments)
        {
            result.Add(new CommentsResponse()
                    {
                        Id = comment.CommentsId,
                        KudosId = comment.KudosId,
                        FromPerson = new GatewayDomain.Person
                        {
                            Id = comment.UserFrom.UserProfileId,
                            Name = comment.UserFrom.DisplayName,
                            Photo = comment.UserFrom.Photo != null ? $"data:image/png;base64,{comment.UserFrom.Photo}" : _defaultProfilePicture
                        },
                        Message = comment.Message,
                        Date = comment.Date,
                        Likes = comment.Likes.Where(l => l.Person != null).Select(x => new GatewayDomain.Person()
                        {
                            Id = x.Person.UserProfileId,
                            Name = x.Person.DisplayName,
                            Photo = x.Person.Photo != null ? $"data:image/png;base64,{x.Person.Photo}" : _defaultProfilePicture
                        }).ToList()
            }
            );
        }



        return result;

      

    }


    [HttpPut(Name ="Update")]
    public async Task<bool> Put ([FromBody]CommentsRequest comments)
    {

        bool succeed = false;

        succeed = await  _commentsService.UpdateComments(new Comments()
        {
            CommentsId = comments.Id,
            KudosId = comments.KudosId,
            Message = comments.Message,
            FromPersonId = comments.FromPersonId,
            Date = comments.Date
        });

        //if (succeed)
        //{
        //    await _commentsMessageSender.MessageUpdated(comments);
        //}

        return succeed;        
    }

    [HttpDelete(Name = "Delete")]
    public async Task<bool> Delete([FromBody] CommentsRequest comments)
    {
        bool succeed = false;

        succeed = await _commentsService.DeleteComments(comments.KudosId, comments.Id);

        if (succeed)
        {
            //we need to get who is receiving the kudos to update their score on the dashboard 
            var kudos = await _kudosService.GetKudosUser(comments.KudosId);

            if (kudos != null)
            {
                await _commentsMessageSender.MessageDeleted(comments, kudos.Recognized);
            }

        }
        

        return succeed;
                
    }
}
