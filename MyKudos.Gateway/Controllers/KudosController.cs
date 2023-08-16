using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Domain.Models;
using GatewayDomain = MyKudos.Gateway.Domain.Models;
using MyKudos.Kudos.Domain.Models;
using MyKudos.Gateway.Helpers;

namespace MyKudos.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class KudosController : Controller
{
    
    private readonly IGraphService _graphService;
    private readonly IKudosService _kudosService;

    private readonly IAgentNotificationService _agentNotificationService;

    private readonly IUserProfileService _userProfileService;

    private string _defaultProfilePicture;

    private IKudosMessageSender _kudosQueue;

    
    public KudosController(IGraphService graphService, 
                           IKudosService kudosService, IKudosMessageSender kudosQueue,
                           IConfiguration configuration,
                           IAgentNotificationService agentNotificationService,
                           IUserProfileService userProfileService)
    {
        
        _graphService = graphService;
        _kudosService = kudosService;
        _agentNotificationService = agentNotificationService;

        _kudosQueue = kudosQueue;

        _userProfileService = userProfileService;

        _defaultProfilePicture = configuration["DefaultProfilePicture"];

    }


    [HttpGet(Name = "GetKudos")]
    public async Task<IEnumerable<KudosResponse>> Get(int pageNumber = 1)
    {
        //get kudos
        var kudos = await _kudosService.GetKudosAsync(pageNumber);


        return KudosHelper.GetKudos(kudos, _defaultProfilePicture);

    }

    /// <summary>
    /// Send a recognition (KUDOS) to one or more person. Each person (sender/receivers) will receive a notification card on Teams as well as their respective managers.
    /// </summary>
    /// <param name="kudos"></param>
    /// <returns></returns>
    [HttpPost(Name = "SendKudos")]
    public async Task<int> PostAsync([FromBody] SendKudosRequest kudos)
    {

        //convert the input to domain model
        var restKudos = new Kudos.Domain.Models.Kudos()
        {
            FromPersonId = kudos.FromPersonId,
            Recognized = kudos.ToPersonId.Select(id => new KudosReceiver { ToPersonId = id }).ToList(),
            RecognitionId = kudos.RecognitionId,
            Message = kudos.Message,
            Date = kudos.Date            
        };

        //Save the Kudos in the database
        int kudosId = await _kudosService.SendAsync(restKudos);

        //get missing information like photos to send the adaptive card and notifications
        var kudosDb = await _kudosService.GetKudosUser(kudosId);

        if (kudosDb != null)
        {
            //get the manager of the person/people that received kudos.            
            var managers = await _userProfileService.GetManagers(restKudos.Recognized.Select(u=> u.ToPersonId).ToArray());


            if (managers == null)
            {
                return kudosId;
            }

            foreach (var userManagerId in managers)
            {

                //send the kudos notification to the Teams Dashboard app
                var queue = _kudosQueue.SendKudosAsync(kudosId, new GatewayDomain.KudosNotification(
                  From: new GatewayDomain.Person
                  {
                      Id = kudosDb.FromPersonId,
                      Name = kudosDb.UserFrom.DisplayName,
                      Photo = $"data:image/png;base64,{kudosDb.UserFrom.Photo}"
                  },
                  Receivers: kudosDb.Recognized.Select(r => 
                                              new GatewayDomain.Person
                                              {
                                                  Id = r.ToPersonId,
                                                  Name = r.Person.DisplayName,
                                                  Photo = $"data:image/png;base64,{r.Person.Photo96x96}"
                                              }
                                          ).ToList(),
                  ManagerId: userManagerId.ManagerId.Value,
                  Message: kudosDb.Message,
                  Reward: new GatewayDomain.Reward(Id: kudosDb.Recognition.RecognitionId, Title: kudosDb.Recognition.Title),
                  SendOn: kudosDb.Date
                  ));

                //send the adaptive card
                await _agentNotificationService.SendNotificationAsync(
                    new Kudos.Domain.Models.KudosNotification(
                          From: new Kudos.Domain.Models.Person()
                          {
                              Id = kudosDb.UserFrom.UserProfileId,
                              Name = kudosDb.UserFrom.DisplayName,
                              Photo = $"data:image/png;base64,{kudosDb.UserFrom.Photo}"
                          }
                          ,
                          Receivers: kudosDb.Recognized.Select(r =>
                                              new Kudos.Domain.Models.Person
                                              {
                                                  Id = r.ToPersonId,
                                                  Name = r.Person.DisplayName,
                                                  Photo = $"data:image/png;base64,{r.Person.Photo96x96}"
                                              }
                                          ).ToList(),
                  ManagerId: userManagerId.ManagerId.Value,
                          Message: kudosDb.Message,
                          Reward: new Kudos.Domain.Models.Reward(kudosDb.Recognition.RecognitionId, kudosDb.Recognition.Title),
                          SendOn: kudosDb.Date)
                    );

                Task.WaitAll(queue);
            }
        }

        return kudosId;
    }

    


}
