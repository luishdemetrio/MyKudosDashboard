﻿using Microsoft.AspNetCore.Mvc;
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

    private readonly IAgentNotificationService _agentNotificationService;

    private List<Models.Recognition> _recognitions;

    public KudosController(IGraphService graphService, IRecognitionService recognitionService, IKudosService kudosService, IAgentNotificationService agentNotificationService)
    {
        
        _graphService = graphService;
        _recognitionService = recognitionService;
        _kudosService = kudosService;

        _agentNotificationService = agentNotificationService;

        _recognitions = _recognitionService.GetRecognitions().ToList();

    }

    [HttpGet(Name = "GetKudos")]
    public async Task<IEnumerable<Models.KudosResponse>> Get()
    {       
        var kudos = _kudosService.GetKudos();

        var from= kudos.Select(u => u.From).Distinct().ToList();

        from.AddRange(kudos.Select(u =>u.To).Distinct());

        List<string> likesId = new();

        //user from likes
        foreach (var k in kudos)
        {
            likesId.AddRange( k.Likes);

        }

        from.AddRange(likesId.Distinct());

        List<GraphUser> users =  await _graphService.GetUserInfoAsync(from.Distinct().ToArray()).ConfigureAwait(true);
                
        var photos = await _graphService.GetUserPhotos(from.Distinct().ToArray()).ConfigureAwait(true);
        
        List<LikeDTO> likes = new();

        foreach (var k in kudos)
        {

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
                                Photo = photo.photo
                            }
                            
                        ));
        }

        var  result =  from kudo in kudos
                        join userTo in users
                            on kudo.To equals userTo.Id
                        join userFrom in users
                            on kudo.From equals userFrom.Id
                        join photoTo in photos
                            on kudo.To equals photoTo.id
                        join photoFrom in photos
                            on kudo.From equals photoFrom.id
                        join rec in _recognitions
                            on kudo.TitleId equals rec.Id                        

                        select new Models.KudosResponse(
                                Id: kudo.Id,
                                From: new Person() { Id = kudo.From, Name = userFrom.DisplayName, Photo = photoFrom.photo },
                                To: new Person() { Id = kudo.To, Name = userTo.DisplayName, Photo = photoTo.photo },
                                Title: rec.Description,
                                Message: kudo.Message,
                                SendOn: kudo.SendOn,
                                Likes: likes.Where( l => l.KudosId == kudo.Id ).Select( l => l.Person )
                            );
        

        return result;


    }

    
    [HttpPost(Name = "SendKudos")]
    public IActionResult Post([FromBody] Models.KudosRequest kudos)
    {

        _kudosService.Send(kudos);

        string userManagerId = _graphService.GetUserManager(kudos.To.Id);


        _agentNotificationService.SendNotification(
            new KudosNotification(                
                From : kudos.From,
                To : kudos.To,                 
                ManagerId : userManagerId,
                Message : kudos.Message,
                Title : kudos.Title,
                SendOn : kudos.SendOn
                ));

        return Ok(kudos);
    }

    


}