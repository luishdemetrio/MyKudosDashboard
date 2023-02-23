using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyKudos.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class KudosController : Controller
{
    
    private readonly IGraphService _graphService;
    private readonly IRecognitionService _recognitionService;
    private readonly IKudosService _kudosService;

    //private readonly IAgentNotificationService _agentNotificationService;

    private IEnumerable<Models.Recognition> _recognitions;

    private IKudosQueue _kudosNotificationQueue;

    
    public KudosController(IGraphService graphService, IRecognitionService recognitionService, 
                           IKudosService kudosService, IKudosQueue kudosQueue)
    {
        
        _graphService = graphService;
        _recognitionService = recognitionService;
        _kudosService = kudosService;

        //_agentNotificationService = agentNotificationService;

        _kudosNotificationQueue= kudosQueue;

        _ = PopulateRecognitionsAsync();

    }

    private async Task PopulateRecognitionsAsync()
    {
        _recognitions = await _recognitionService.GetRecognitionsAsync().ConfigureAwait(false);
    }


    [HttpGet(Name = "GetKudos")]
    public async Task<IEnumerable<Models.KudosResponse>> Get()
    {       
        var kudos = await _kudosService.GetKudosAsync().ConfigureAwait(false);

        var from= kudos.Select(u => u.FromPersonId).Distinct().ToList();

        from.AddRange(kudos.Select(u =>u.ToPersonId).Distinct());

        List<string> likesId = new();

        //user from likes
        foreach (var k in kudos)
        {
            if (k.Likes != null)
                likesId.AddRange( k.Likes);

        }

        from.AddRange(likesId.Distinct());

        List<Models.GraphUser> users =  await _graphService.GetUserInfo(from.Distinct().ToArray()).ConfigureAwait(true);
                
        var photos = await _graphService.GetUserPhotos(from.Distinct().ToArray()).ConfigureAwait(true);
        
        List<LikeDTO> likes = new();

        foreach (var k in kudos)
        {
            if (k.Likes != null)
                likes.AddRange( from like in k.Likes
                        join u in users
                            on like equals u.Id
                        join photo in photos
                            on like equals photo.id
                        select new LikeDTO(
                        
                            KudosId : k.Id,
                            Person : new Person()
                            {
                                Id = like,
                                Name = u.DisplayName,
                                Photo = $"data:image/png;base64,{photo.photo}"
                            }
                            
                        ));
        }

        var  result =  from kudo in kudos
                        join userTo in users
                            on kudo.ToPersonId equals userTo.Id
                        join userFrom in users
                            on kudo.FromPersonId equals userFrom.Id
                        join photoTo in photos
                            on kudo.ToPersonId equals photoTo.id
                        join photoFrom in photos
                            on kudo.FromPersonId equals photoFrom.id
                        join rec in _recognitions
                            on kudo.TitleId equals rec.Id                        
                        orderby kudo.Date descending

                        select new Models.KudosResponse(
                                Id: kudo.Id,
                                From: new Person() { Id = kudo.FromPersonId, Name = userFrom.DisplayName, Photo = $"data:image/png;base64,{photoFrom.photo}" },
                                To: new Person() { Id = kudo.ToPersonId, Name = userTo.DisplayName, Photo = $"data:image/png;base64,{photoTo.photo}" },
                                Title: rec.Description,
                                Message: kudo.Message,
                                SendOn: kudo.Date,
                                Likes: likes.Where( l => l.KudosId == kudo.Id ).Select( l => l.Person )
                            );
        

        return result;


    }

    
    [HttpPost(Name = "SendKudos")]
    public async Task<string> PostAsync([FromBody] Models.KudosRequest kudos)
    {

       string kudosId = await _kudosService.SendAsync(kudos).ConfigureAwait(false);

        string userManagerId = await _graphService.GetUserManagerAsync(kudos.To.Id);

        await _kudosNotificationQueue.SendAsync(new KudosNotification(
                From: kudos.From,
                To: kudos.To,
                ManagerId: userManagerId,
                Message: kudos.Message,
                Title: kudos.Title,
                SendOn: kudos.SendOn
                ));
                

        return kudosId;
    }

    


}
