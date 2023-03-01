﻿using Microsoft.AspNetCore.Mvc;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Models;
using Newtonsoft.Json.Linq;

namespace MyKudos.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class ContributorsController : Controller
{
    private readonly IGamificationService _gamificationService;
    private readonly IGraphService _graphService;

    public ContributorsController(IGamificationService gamificationService, IGraphService graphService)
    {
        _gamificationService = gamificationService;
        _graphService = graphService;
    }

    [HttpGet(Name = "GetTopContributors")]
    public async Task<IEnumerable<TopContributors>> Get()
    {

        var scores = await _gamificationService.GetTopUserScoresAsync(20);

        var userIds = scores.Select(s => s.UserId).Distinct().ToArray();

        List<Models.GraphUser> users = await _graphService.GetUserInfo(userIds).ConfigureAwait(true);

        var photos = await _graphService.GetUserPhotos(userIds).ConfigureAwait(true);

        var result = from score in scores
                     join photo in photos
                        on score.UserId equals photo.id
                     join user in users
                        on score.UserId equals user.Id
                     select new TopContributors()
                     {
                         Name = user.DisplayName,
                         Photo = $"data:image/png;base64,{photo.photo}",
                         Score = score.Score
                     };

        return result;

    }
}