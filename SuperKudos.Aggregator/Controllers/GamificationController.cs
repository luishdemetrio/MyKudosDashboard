//using Microsoft.AspNetCore.Mvc;
//using SuperKudos.Aggregator.Interfaces;
//using SuperKudos.Aggregator.Domain.Models;

//namespace SuperKudos.Aggregator.Controllers;

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
