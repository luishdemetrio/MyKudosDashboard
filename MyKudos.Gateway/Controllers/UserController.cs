﻿using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Domain.Models;

namespace MyKudos.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{


    private IGraphService _graphService;

    private string _defaultProfilePicture;

    public UserController(IGraphService graphService, IConfiguration configuration)
    {
        _graphService = graphService;

        _defaultProfilePicture = configuration["DefaultProfilePicture"];
    }


    [HttpGet(Name = "GetUser/{name}")]
    public async Task<IEnumerable<Person>> GetUsersAsync(string name)
    {

        var users = new List<Person>();

        var graphUsers = await _graphService.GetUsers(name);

        if (graphUsers.Count() == 0)
            return new List<Person>();

        // var photos = await _graphService.GetUserPhotos(graphUsers.value.Select(u => u.Id).Take(20).ToArray());

        var photos = await _graphService.GetUserPhotos(graphUsers.Select(u => u.Id).ToArray());


        return (from graphUser in graphUsers
                join photo in photos
                    on graphUser.Id equals photo.id into ph
                from p in ph.DefaultIfEmpty()
                select new Person() {
                        Id= graphUser.Id,
                        Name= graphUser.DisplayName,
                        Photo= string.IsNullOrEmpty(p?.photo)? _defaultProfilePicture : "data:image/png;base64," + p.photo
                       });

    }

   
}
