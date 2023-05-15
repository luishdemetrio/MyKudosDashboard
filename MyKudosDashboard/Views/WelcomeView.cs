﻿using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;
using Newtonsoft.Json;

namespace MyKudosDashboard.Views;

public class WelcomeView : IWelcomeView
{

    private ServiceBusClient _serviceBusClient;
    private ServiceBusProcessor _serviceBusScoreProcessor;

    private string _userId;

    private IKudosGateway _kudosGateway;

    private IUserGateway _userGateway;

    private string _serviceBusConnectionString;

    public WelcomeView(IKudosGateway gatewayService, IConfiguration configuration, IUserGateway userGateway)
    {
        _kudosGateway = gatewayService;

        _serviceBusConnectionString = configuration["KudosServiceBus_ConnectionString"];

        _serviceBusClient = new ServiceBusClient(_serviceBusConnectionString);
        _userGateway = userGateway;
    }

    public IWelcomeView.UpdateScoreCallBack ScoreCallback { get ; set ; }

    public async Task<string> GetUserPhoto(string userId)
    {

        return await _userGateway.GetUserPhoto(userId);
    }


    public void RegisterForUserScoreUpdate(string userId)
    {
        //its used later to control the score
        _userId = userId;

        ServiceBusScoreProcessor(_userId);

    }

    private async Task CreateATopicIfItDoesntExistAsync(string topicName, string subscriptionName)
    {
        //create a topic if it doesnt exist
        var serviceBusAdminClient = new ServiceBusAdministrationClient(_serviceBusConnectionString);

        if (!await serviceBusAdminClient.TopicExistsAsync(topicName))
        {
            await serviceBusAdminClient.CreateTopicAsync(topicName);
        }

        //create a temp subscription for the user

        if (!await serviceBusAdminClient.SubscriptionExistsAsync(topicName, subscriptionName))
        {
            var options = new CreateSubscriptionOptions(topicName, subscriptionName)
            {
                AutoDeleteOnIdle = TimeSpan.FromHours(1)
            };

            await serviceBusAdminClient.CreateSubscriptionAsync(options);
        }
    }

    private async void ServiceBusScoreProcessor(string userId)
    {

        await CreateATopicIfItDoesntExistAsync("dashboardscoreupdated", userId);

        _serviceBusScoreProcessor = _serviceBusClient.CreateProcessor("dashboardscoreupdated", userId);

        _serviceBusScoreProcessor.ProcessMessageAsync += _serviceBusScoreProcessor_ProcessMessageAsync;

        _serviceBusScoreProcessor.ProcessErrorAsync += _serviceBusScoreProcessor_ProcessErrorAsync;

        await _serviceBusScoreProcessor.StartProcessingAsync();
    }

    private async Task _serviceBusScoreProcessor_ProcessErrorAsync(ProcessErrorEventArgs arg)
    {
        await Task.CompletedTask;
    }

    private async Task _serviceBusScoreProcessor_ProcessMessageAsync(ProcessMessageEventArgs arg)
    {
        //retrive the message body

        var score = JsonConvert.DeserializeObject<UserScore>(arg.Message.Body.ToString());

        if ((score != null) && (score.UserId == _userId) )
        {
            ScoreCallback?.Invoke(score);
        }

        await arg.CompleteMessageAsync(arg.Message);
    }
}
