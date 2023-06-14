﻿using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.MSGraph.Api.Interfaces;
using MyKudos.MSGraph.Api.Models;

namespace MyKudos.MSGraph.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AllUsersController : Controller
{

    private readonly IGraphService _graphService;

    private readonly string[] _emailDomain;

    private readonly IUserProfileRepository _userProfileRepository;

    public AllUsersController(IGraphService graphService, IConfiguration configuration,
                              IUserProfileRepository userProfileRepository)
    {
        _graphService = graphService;

        _emailDomain = configuration["EmailDomain"].ToString().Split(",");

        _userProfileRepository = userProfileRepository;
    }

    [HttpGet(Name = "GetAllUsers")]
    public async Task<bool> GetAllUsers()
    {

        List<GraphUser> result = new();

        
        await _graphService.GetAllUsers(_userProfileRepository, _emailDomain);
        

        return true;
    }
}
