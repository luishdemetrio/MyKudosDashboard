using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Logging;
using MyKudosDashboard.Interfaces;
using MyKudosDashboard.MessageSender;
using MyKudosDashboard.Models;
using Newtonsoft.Json;

namespace MyKudosDashboard.Views;

public class WelcomeView : IWelcomeView
{

    private ServiceBusClient _serviceBusClient;
    private ServiceBusProcessor _serviceBusScoreProcessor;

    private string _userId;


    private IUserGateway _userGateway;

    private static string _updatedScoreDashboard = string.Empty;
    private static string _updatedScoreSamePersonDashboard = string.Empty;

    private ServiceBusSubscriberHelper _subscriberUserScore;

    public WelcomeView(IKudosGateway gatewayService, IConfiguration configuration, IUserGateway userGateway, ILogger<WelcomeView> logger)
    {

        _userGateway = userGateway;

        _updatedScoreDashboard = configuration["KudosServiceBus_ScoreUpdatedDashboard"];

        _updatedScoreSamePersonDashboard = configuration["KudosServiceBus_ScoreUpdatedSamePersonDashboard"];

        _subscriberUserScore = new ServiceBusSubscriberHelper(configuration, logger);
    }



    public IWelcomeView.UpdateScoreCallBack ScoreCallback { get ; set ; }

    public async Task<string> GetUserPhoto(string userId)
    {

        return await _userGateway.GetUserPhoto(userId);
    }

    public void RegisterForUserScoreUpdate(string userId)
    {
        _userId = userId;

        SubscribeUserScoreUpdate(userId);

        SubscribeUserScoreSamePersonUpdate(userId);
    }

    private void SubscribeUserScoreUpdate(string subscriptionName)
    {
        var config = new ServiceBusProcessorConfig
        {
            DashboardName = _updatedScoreDashboard,
            SubscriptionName = subscriptionName,
            MessageProcessor = async arg =>
            {
                //retrive the message body
                var userScore = JsonConvert.DeserializeObject<UserScore>(arg.Message.Body.ToString());

                if ((userScore != null) && (userScore.Id == _userId))
                {
                    await ScoreCallback?.Invoke(userScore);
                }

                await arg.CompleteMessageAsync(arg.Message).ConfigureAwait(true);
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
                //retrive the message body
                var userScore = JsonConvert.DeserializeObject<UserScore>(arg.Message.Body.ToString());

                if ((userScore != null) && (userScore.Id == _userId))
                {
                    await ScoreCallback?.Invoke(userScore);
                }

                await arg.CompleteMessageAsync(arg.Message).ConfigureAwait(true);
            }
        };

        _subscriberUserScore.ServiceBusProcessor(config);
    }
}
