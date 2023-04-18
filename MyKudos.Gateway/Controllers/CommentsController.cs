﻿using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Models;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : Controller
{

    private readonly ICommentsService _commentsService;

    private ICommentsMessageSender _commentsMessageSender;

    private readonly IGraphService _graphService;

    public CommentsController(ICommentsService commentsService, ICommentsMessageSender commentsMessageSender,
                              IGraphService graphService)
    {

        _commentsService = commentsService;

        _commentsMessageSender = commentsMessageSender;
        _graphService = graphService;
    }

    [HttpPost(Name = "SendMessage")]
    public async Task<string> Post(CommentsRequest comments)
    {
        string commentId = string.Empty;

        commentId =  await _commentsService.SendCommentsAsync(new Comments()
        {
            KudosId = comments.KudosId,
            Message = comments.Message,
            FromPersonId = comments.FromPersonId,
            Date = comments.Date
        });

        if (!string.IsNullOrEmpty(commentId))
        {
            comments.Id = commentId;
            await _commentsMessageSender.MessageSent(comments);
        }

        return commentId;
    }

    [HttpGet(Name = "GetComments")]
    public async Task<IEnumerable<CommentsResponse>> Get(string kudosId)
    {

        var result = new List<CommentsResponse>();

        var comments =  await _commentsService.GetComments(kudosId);

        if (comments != null)
        {
            var peopleIds = new List<string>();

            foreach (var comment in comments)
            {
                peopleIds.AddRange(comment.Likes.Distinct());

                if (!peopleIds.Contains(comment.FromPersonId))
                {
                    peopleIds.Add(comment.FromPersonId);
                }
            }

            List<Models.GraphUser> users = await _graphService.GetUserInfo(peopleIds.Distinct().ToArray()).ConfigureAwait(true);

            var photos = await _graphService.GetUserPhotos(peopleIds.Distinct().ToArray()).ConfigureAwait(true);



            List<LikeMessage> likes = new();

            foreach (var comment in comments)
            {
                if (comment.Likes != null)
                    likes.AddRange(from like in comment.Likes
                                   join u in users
                                       on like equals u.Id
                                   join photo in photos
                                       on like equals photo.id
                                   select new LikeMessage(

                                       MessageId: comment.Id.ToString(),
                                       Person: new Models.Person()
                                       {
                                           Id = like,
                                           Name = u.DisplayName,
                                           Photo = $"data:image/png;base64,{photo.photo}"
                                       }

                                   ));
            }


            result = (from comment in comments
                    join user in users
                       on comment.FromPersonId equals user.Id
                    join photo in photos
                        on user.Id equals photo.id
                    select new CommentsResponse()
                    {
                        Id = comment.Id.ToString(),
                        KudosId = comment.KudosId,
                        FromPerson = new Models.Person {Id = user.Id, Name = user.DisplayName, Photo = $"data:image/png;base64,{photo.photo}" },
                        Message = comment.Message,
                        Date = comment.Date,
                        Likes =  likes.Where(l => l.MessageId == comment.Id.ToString()).Select(l => l.Person).ToList()
                    }).ToList();



        }
        


        return result;

    }


    [HttpPut(Name ="Update")]
    public async Task<bool> Put ([FromBody]CommentsRequest comments)
    {

        bool succeed = false;

        succeed = await  _commentsService.UpdateComments(new Comments()
        {
            Id = Guid.Parse(comments.Id),
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

        succeed = await _commentsService.DeleteComments(comments.KudosId, comments.Id.ToString());

        if (succeed)
        {   
            await _commentsMessageSender.MessageDeleted(comments);
        }
        

        return succeed;
                
    }
}
