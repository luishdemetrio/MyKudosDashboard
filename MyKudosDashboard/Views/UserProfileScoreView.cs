﻿using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.Interfaces;
using MyKudosDashboard.MessageSender;
using Newtonsoft.Json;

namespace MyKudosDashboard.Views;

public class UserProfileScoreView : IUserProfileScoreView
{

    private IGamificationGateway _gamificationGateway;

    public IUserProfileScoreView.UpdateScoreCallBack UserScoreCallback { get; set; }

    private static string _updatedScoreDashboard = string.Empty;
    private static string _updatedScoreSamePersonDashboard = string.Empty;

    private ServiceBusSubscriberHelper _subscriberUserScore;

    private static SemaphoreSlim _semaphoreScore = new SemaphoreSlim(1, 1);
    private static SemaphoreSlim _semaphoreSamePersonScore = new SemaphoreSlim(1, 1);

    

    public UserProfileScoreView(IGamificationGateway gamificationGateway, IConfiguration configuration, ILogger<UserProfileScoreView> logger)
    {
        _gamificationGateway = gamificationGateway;

        _updatedScoreDashboard = configuration["KudosServiceBus_ScoreUpdatedDashboard"];

        _updatedScoreSamePersonDashboard = configuration["KudosServiceBus_ScoreUpdatedSamePersonDashboard"];

        _subscriberUserScore = new ServiceBusSubscriberHelper(configuration, logger);
    }


    public async Task<UserPointScore> GetUserScore(string userId)
    {
        return await _gamificationGateway.GetUserScoreAsync(userId);
    }


    public void RegisterForLiveUpdates(string userId)
    {
    

        SubscribeUserScoreUpdate(userId);

        SubscribeUserScoreSamePersonUpdate(userId);
    }

    private void SubscribeUserScoreUpdate(string subscriptionName)
    {
        var config = new ServiceBusProcessorConfig
        {
            DashboardName = _updatedScoreDashboard,
            SubscriptionName = _subscriberUserScore.GetInstanceId(subscriptionName),
            MessageProcessor = async arg =>
            {
                
                
                    await _semaphoreScore.WaitAsync();

                try
                {
                    var userScore = JsonConvert.DeserializeObject<UserPointScore>(arg.Message.Body.ToString());

                    if (userScore != null) 
                    {
                        UserScoreCallback?.Invoke(userScore);
                    }
                }
                finally
                {
                    _semaphoreScore.Release();
                }

                await arg.CompleteMessageAsync(arg.Message);
            }
        };

        _subscriberUserScore.ServiceBusProcessor(config);
    }

    private void SubscribeUserScoreSamePersonUpdate(string subscriptionName)
    {
        var config = new ServiceBusProcessorConfig
        {
            DashboardName = _updatedScoreSamePersonDashboard,
            SubscriptionName = subscriptionName,
            MessageProcessor = async arg =>
            {
              

                    await _semaphoreSamePersonScore.WaitAsync();

                try
                {
                    //retrive the message body
                    var userScore = JsonConvert.DeserializeObject<UserPointScore>(arg.Message.Body.ToString());

                    if (userScore != null)
                    {
                        UserScoreCallback?.Invoke(userScore);
                    }
                }
                finally
                {
                    _semaphoreSamePersonScore.Release();
                }

                    
                

                await arg.CompleteMessageAsync(arg.Message).ConfigureAwait(true);
            }
        };

        _subscriberUserScore.ServiceBusProcessor(config);
    }

}
