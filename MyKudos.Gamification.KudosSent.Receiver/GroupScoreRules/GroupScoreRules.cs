using Microsoft.Extensions.Configuration;
using MyKudos.Gamification.Domain.Models;
using MyKudos.Gamification.Receiver.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MyKudos.Gamification.Receiver;

public class GroupScoreRules : IGroupScoreRules
{

    private readonly IUserScoreService _userScoreService;

    private readonly IUserKudosService _userKudosService;
    private readonly IGroupUserScoreService _groupUserScoreService;

  

    private readonly int _groupCompletedScore;

    private readonly int _allGroupCompletedScore;

    private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public GroupScoreRules(IUserScoreService userScoreService,
                           IUserKudosService userKudosService,
                           IConfiguration configuration,
                           IGroupUserScoreService groupUserScoreService)
    {
        _userScoreService = userScoreService;
        _groupUserScoreService = groupUserScoreService;

        _userKudosService = userKudosService;

        _groupCompletedScore = int.Parse(configuration["GroupCompletedScore"]);
        _allGroupCompletedScore = int.Parse(configuration["AllGroupCompletedScore"]);
    }

    private int GetGroupScore(int value)
    {
        return value >= 3 ? _groupCompletedScore : 0;
    }

    public async Task<UserScore> UpdateGroupScoreAsync(UserScore pUserScore)
    {


        UserScore score;


        await _semaphore.WaitAsync();

        
        try
        {
            await _userScoreService.SetUserScoreAsync(pUserScore).ConfigureAwait(false);

             score = await _userScoreService.GetUserScoreAsync(pUserScore.Id.ToString());

            try
            {
                if (score != null)
                {
                    var kudosGroup = await _userKudosService.GetUserKudosByCategory(pUserScore.Id.ToString());

                    if (kudosGroup != null)
                    {
                        foreach (var item in kudosGroup)
                        {
                            switch (item.ValueCodeGroup)
                            {

                                case 1:
                                    score.GroupOne = GetGroupScore(item.Count); break;

                                case 2:
                                    score.GroupTwo = GetGroupScore(item.Count); break;

                                case 3:
                                    score.GroupThree = GetGroupScore(item.Count); break;

                                case 4:
                                    score.GroupFour = GetGroupScore(item.Count); break;

                                case 5:
                                    score.GroupFive = GetGroupScore(item.Count); break;

                            }
                        }

                        var all = score.GroupOne * score.GroupTwo * score.GroupThree * score.GroupFour * score.GroupFive;

                        if ((all != 0) && (all != null))
                        {
                            score.GroupAll = _allGroupCompletedScore;

                            await _groupUserScoreService.UpdateGroupScoreAsync(score);
                        }
                    }


                }
            }
            catch { }
        }
        finally
        {
            _semaphore.Release();
        }


        return score;
    }
}
