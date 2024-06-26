﻿using Microsoft.AspNetCore.Mvc;
using MyKudos.Kudos.App.Interfaces;
using MyKudos.Kudos.Domain.Interfaces;
using MyKudos.Kudos.Domain.Models;

namespace MyKudos.Kudos.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserPointsController : Controller
{

    private readonly IUserPointsRepository _userPointsRepository;

    public UserPointsController(IUserPointsRepository userPointsRepository)
    {
        _userPointsRepository = userPointsRepository;
    }

    [HttpGet("GetUserPoints/{userId},{justMyTeam},{sentOnYear}")]
    public UserPointScore GetUserPoints(Guid userId, bool justMyTeam = false,  int? sentOnYear = null)
    {

        return _userPointsRepository.GetUserScore(userId, justMyTeam, sentOnYear);
    }

  
}
