using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Domain.Models;
using GatewayDomain = MyKudos.Gateway.Domain.Models;
using MyKudos.Kudos.Domain.Models;
using MyKudos.Gateway.Helpers;
using MyKudos.MSGraph.gRPC;


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

    /// <summary>
    /// Returns the Kudos by pageNumber and opnionally by manager
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="managerId"></param>
    /// <returns></returns>
    [HttpGet(Name = "GetKudos")]
    public async Task<IEnumerable<KudosResponse>> Get(int pageNumber , int pageSize, Guid? managerId = null, 
                                                      int? sentOnYear = null)
    {
        //get kudos
        var kudos = await _kudosService.GetKudosAsync(pageNumber, pageSize, managerId, sentOnYear);


        return KudosHelper.GetKudos(kudos, _defaultProfilePicture, false);

    }

    /// <summary>
    /// Returns the Kudos by pageNumber and opnionally by manager
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="managerId"></param>
    /// <returns></returns>
    [HttpGet("GetKudosByName/{name},{pageSize},{fromNumberOfDays}")]
    public async Task<IEnumerable<KudosResponse>> Get(string name, int pageSize, int fromNumberOfDays=0, bool useSmallPhoto = true)
    {
        //get kudos
        var kudos = await _kudosService.GetKudosByName(name, pageSize, fromNumberOfDays);


        return KudosHelper.GetKudos(kudos, _defaultProfilePicture, useSmallPhoto);

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
            Recognized = kudos.ToPersonId.Select(id => new KudosReceiver { ToPersonId = id }).DistinctBy(p => p.ToPersonId).ToList(),
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
            var managers = await _userProfileService.GetManagers(restKudos.Recognized.Select(u => u.ToPersonId).ToArray());

            var notification = new GatewayDomain.KudosNotification
                (
                    From: new GatewayDomain.Person
                    {
                        Id = kudosDb.FromPersonId,
                        Name = kudosDb.UserFrom.DisplayName,
                        Photo = kudosDb.UserFrom.Photo != null ? $"data:image/png;base64,{kudosDb.UserFrom.Photo}" : _defaultProfilePicture
                    },
                    Receivers: kudosDb.Recognized.Select(r =>
                                new GatewayDomain.Person
                                {
                                    Id = r.ToPersonId,
                                    Name = r.Person.DisplayName,
                                    Photo = r.Person.Photo96x96 != null ? $"data:image/png;base64,{r.Person.Photo96x96}" : _defaultProfilePicture,
                                    GivenName = r.Person.GivenName
                                }).ToList(),
                    Reward: new GatewayDomain.Reward(Id: kudosDb.Recognition.RecognitionId, Title: kudosDb.Recognition.Title),
                    Message: kudosDb.Message,
                    SendOn: kudosDb.Date,
                    Recipients: managers.Where(m => m.ManagerId != null).Select(m => m.ManagerId.Value).Distinct().ToList()
                );

            //the person who sent the kudos must receive it
            notification.Recipients.Add(kudosDb.FromPersonId);

            //the receivers must be notified too
            notification.Recipients.AddRange(kudosDb.Recognized.Select(r => r.ToPersonId));

            //send the kudos notification to the Teams Dashboard app
            await _kudosQueue.SendKudosAsync(kudosId, notification);

            //send the adaptive card
            await _agentNotificationService.SendNotificationAsync(notification);



        }

        return kudosId;
    }

    [HttpPut(Name = "UpdateKudos")]
    public async Task<bool> UpdateKudos([FromBody] GatewayDomain.KudosMessage kudos)
    {
        bool result = false;

        result = await _kudosService.UpdateKudos(new Kudos.Domain.Models.KudosMessage(kudos.KudosId, kudos.Message));

        if (result)
        {
            //send the kudos notification to the Teams Dashboard app
            await _kudosQueue.KudosUpdated(kudos);
        }
        
        return result;
        
    }

    [HttpDelete(Name = "DeleteKudos")]
    public async Task<bool> DeleteKudos([FromBody] int kudosId)
    {
        bool result = false;

        //Before deleting, I fetch the kudos data and update the scores on dashboard of the affected users
        var kudos = await _kudosService.GetKudosUser(kudosId);

        //now I need to delete the kudos before sending the notification; the notification message will update the afected users with the new score
        result = await _kudosService.DeleteKudos(kudosId);

        if (result)
        {
            //send the kudos notification to the Teams Dashboard app to update the score and remove the kudos
            await _kudosQueue.KudosDeleted(kudosId, kudos);
        }
        

        

        return result;
    }


}
