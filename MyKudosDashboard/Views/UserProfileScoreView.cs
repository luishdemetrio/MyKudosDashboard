using MyKudosDashboard.Interfaces;
using MyKudosDashboard.MessageSender;
using MyKudosDashboard.Models;
using Newtonsoft.Json;

namespace MyKudosDashboard.Views;

public class UserProfileScoreView : IUserProfileScoreView
{

    private IGamificationGateway _gamificationGateway;

    public IUserProfileScoreView.UpdateScoreCallBack UserScoreCallback { get; set ; }

    private static string _updatedScoreDashboard = string.Empty;
    private ServiceBusSubscriberHelper _subscriberUserScore;

    public UserProfileScoreView(IGamificationGateway gamificationGateway, IConfiguration configuration)
    {
        _gamificationGateway = gamificationGateway;

        _updatedScoreDashboard = configuration["KudosServiceBus_ScoreUpdatedDashboard"];

        _subscriberUserScore = new ServiceBusSubscriberHelper(configuration);
    }


    public async Task<UserScore> GetUserScore(string userId)
    {
        return await _gamificationGateway.GetUserScoreAsync(userId);
    }

    public void RegisterForLiveUpdates(string userId)
    {
        SubscribeUserScoreUpdate(userId);
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

                if (userScore != null)
                {
                    UserScoreCallback?.Invoke(userScore);
                }

                await arg.CompleteMessageAsync(arg.Message).ConfigureAwait(true);
            }
        };

        _subscriberUserScore.ServiceBusProcessor(config);
    }
}
