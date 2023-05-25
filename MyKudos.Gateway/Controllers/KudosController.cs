using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Domain.Models;
using MyKudos.Kudos.Domain.Models;
using GatewayDomain = MyKudos.Gateway.Domain.Models;

namespace MyKudos.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class KudosController : Controller
{
    
    private readonly IGraphService _graphService;
    private readonly IRecognitionService _recognitionService;
    private readonly IKudosService _kudosService;

    //private readonly IAgentNotificationService _agentNotificationService;

    private IEnumerable<GatewayDomain.Recognition> _recognitions;

    private IKudosMessageSender _kudosQueue;

    
    public KudosController(IGraphService graphService, IRecognitionService recognitionService, 
                           IKudosService kudosService, IKudosMessageSender kudosQueue)
    {
        
        _graphService = graphService;
        _recognitionService = recognitionService;
        _kudosService = kudosService;

        //_agentNotificationService = agentNotificationService;

        _kudosQueue= kudosQueue;

        _ = PopulateRecognitionsAsync();

    }

    private async Task PopulateRecognitionsAsync()
    {
        _recognitions = await _recognitionService.GetRecognitionsAsync().ConfigureAwait(false);
    }


    [HttpGet(Name = "GetKudos")]
    public async Task<IEnumerable<KudosResponse>> Get(int pageNumber = 1)
    {
        //get kudos
        var kudos = await _kudosService.GetKudosAsync(pageNumber).ConfigureAwait(false);

        //get distinct people who sent
        var from = kudos.Select(u => u.FromPersonId).Distinct().ToList();

        //get distinct people who received
        from.AddRange(kudos.Select(u => u.ToPersonId).Distinct());

        //get distinct people who liked
        List<string> likesId = kudos
                    .SelectMany(kl => kl.Likes)
                    .Select(like => like.PersonId)
                    .Distinct()
                    .ToList();

        from.AddRange(likesId.Distinct());



        List<GraphUser> users = await _graphService.GetUserInfo(from.Distinct().ToArray()).ConfigureAwait(true);

        var photos = await _graphService.GetUserPhotos(from.Distinct().ToArray()).ConfigureAwait(true);

        List<LikeDTO> likes = new();

        foreach (var k in kudos)
        {
            if (k.Likes != null)
                likes.AddRange(from like in k.Likes
                               join u in users
                                   on like.PersonId equals u.Id
                               join photo in photos
                                   on like.PersonId equals photo.id
                               select new LikeDTO(

                                   KudosId: k.KudosId,
                                   Person: new GatewayDomain.Person()
                                   {
                                       Id = like.PersonId,
                                       Name = u.DisplayName,
                                       Photo = $"data:image/png;base64,{photo.photo}"
                                   }

                               ));
        }

        var result = from kudo in kudos
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

                     select new KudosResponse() {
                         Id = kudo.KudosId,
                         From = new GatewayDomain.Person() { Id = kudo.FromPersonId, Name = userFrom.DisplayName, Photo = $"data:image/png;base64,{photoFrom.photo}" },
                         To = new GatewayDomain.Person() { Id = kudo.ToPersonId, Name = userTo.DisplayName, Photo = $"data:image/png;base64,{photoTo.photo}" },
                         Title= rec.Title,
                        Message = kudo.Message,
                        SendOn = kudo.Date,
                        Likes= likes.Where(l => l.KudosId == kudo.KudosId).Select(l => l.Person).ToList(),
                        Comments=  (kudo.Comments is null) ? new List<int>() : kudo.Comments.Select(c=> c.CommentsId).ToList()
                     };
        

        return result;


    }

    
    [HttpPost(Name = "SendKudos")]
    public async Task<int> PostAsync([FromBody] KudosRequest kudos)
    {

        var restKudos = new Kudos.Domain.Models.Kudos()
        {
            FromPersonId = kudos.From.Id,
            ToPersonId = kudos.To.Id,
            TitleId = kudos.Reward.Id,
            Message = kudos.Message,
            Date = kudos.SendOn
        };

        int kudosId = await _kudosService.SendAsync(restKudos);

        string userManagerId = await _graphService.GetUserManagerAsync(kudos.To.Id);


        var queue = _kudosQueue.SendKudosAsync(kudosId, new GatewayDomain.KudosNotification(
          From: kudos.From,
          To: kudos.To,
          ManagerId: userManagerId,
          Message: kudos.Message,
          Reward: kudos.Reward,
          SendOn: kudos.SendOn
          ));

        Task.WaitAll(queue);

        return kudosId;
    }

    


}
