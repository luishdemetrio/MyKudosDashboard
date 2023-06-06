//using Microsoft.AspNetCore.Mvc;
//using MyKudos.Gateway.Interfaces;
//using MyKudos.Gateway.Domain.Models;

//namespace MyKudos.Gateway.Controllers;

//[ApiController]
//[Route("[controller]")]
//public class GamificationController : Controller
//{

//    private readonly IGamificationService _gamificationService;

//    public GamificationController(IGamificationService gamificationService)
//    {
//        _gamificationService = gamificationService;
//    }

//    [HttpGet(Name = "GetGamificationScore")]
//    public async Task<UserScore> Get(string userId)
//    {
//        return await _gamificationService.GetUserScoreAsync(userId);
//    }
//}
