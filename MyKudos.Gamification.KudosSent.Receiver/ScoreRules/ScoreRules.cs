using Microsoft.Extensions.Configuration;
using MyKudos.Gamification.Domain.Models;
using MyKudos.Gamification.Receiver.Interfaces;
using System.Threading.Tasks;

namespace MyKudos.Gamification.Receiver.ScoreRules;

public sealed class ScoreRules
{

    private readonly IUserScoreService _userScoreService;
    private string _kudosSendScore;


    public ScoreRules(IUserScoreService userScoreService, IConfiguration configuration)
    {
        _userScoreService = userScoreService;

        _kudosSendScore = configuration["KudosSendScore"];
    }

    public async Task<UserScore> UpdateUserScoreAsync(string pUserId)
    {

        await _userScoreService.SetUserScoreAsync(
               new UserScore()
               {
                   UserId = pUserId,
                   KudosSent = 1,
                   Score = int.Parse(_kudosSendScore)
               });


        var score = await _userScoreService.GetUserScoreAsync(pUserId);



        return score;
    }
}
