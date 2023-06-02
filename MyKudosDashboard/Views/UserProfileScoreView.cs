using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.EventGrid;
using MyKudosDashboard.EventHub;
using MyKudosDashboard.Interfaces;

namespace MyKudosDashboard.Views;

public class UserProfileScoreView : IUserProfileScoreView, IObserverEventHub<UserPointScore>
{

    private IGamificationGateway _gamificationGateway;

    public IUserProfileScoreView.UpdateScoreCallBack UserScoreCallback { get; set; }


    private IConfiguration _configuration;

    public UserProfileScoreView(IGamificationGateway gamificationGateway, IConfiguration configuration,
                                ILogger<UserProfileScoreView> logger,
                                IEventHubReceived<UserPointScore> eventHubUserPointsReceived
                                )
    {
        _gamificationGateway = gamificationGateway;

        eventHubUserPointsReceived.Attach(this);
        

        _configuration = configuration;

     //   _eventHubScore = eventHubScore;

        //_eventHubScore = new EventHubConsumerHelper<UserPointScore>(
        //                       _configuration["EventHub_ScoreConnectionString"],
        //                       _configuration["EventHub_ScoreName"]
        //                       );

        //_eventHubScore.UpdateCallback += (score => {
        //    UserScoreCallback?.Invoke(score);
        //});

        //_eventHubScore.Start();


    }

    //private async Task RegisterUpdateScore()
    //{
    //    _eventHubScore = new EventHubConsumerHelper<UserPointScore>(
    //                            _configuration["EventHub_ScoreConnectionString"],
    //                            _configuration["EventHub_ScoreName"]
    //                            );

    //    _eventHubScore.UpdateCallback += (score => {
    //        UserScoreCallback?.Invoke(score);
    //    });

    //    await _eventHubScore.Start();
    //}


    public async Task<UserPointScore> GetUserScore(string userId)
    {
        return await _gamificationGateway.GetUserScoreAsync(userId);
    }

   
    public void NotifyUpdate(UserPointScore score)
    {
        UserScoreCallback?.Invoke(score);
    }
}
