using Azure.Messaging.ServiceBus.Administration;
using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Models;
using MyKudos.MessageSender.Interfaces;

namespace MyKudos.Gateway.Queues;

public class KudosMessageSender : IKudosMessageSender
{

    private IMessageSender _messageSender;

    private static string _connectionString = string.Empty;
    private static string _notificationTopicName = string.Empty;

    private static string _gamificationKudosSentTopicName = string.Empty;
    private static string _gamificationKudosReceivedTopicName = string.Empty;

    private static string _gamificationLikeSentTopicName = string.Empty;
    private static string _gamificationLikeReceivedTopicName = string.Empty;

    private static string _gamificationUndolikeSentTopicName = string.Empty;
    private static string _gamificationUndolikeReceivedTopicName = string.Empty;

    private static string _kudosSentDashboard = string.Empty;

    private static string _likeSentDashboard = string.Empty;
    private static string _likeUndoDashboard = string.Empty;

    private static string _gamificationLikeSentSamePersonTopicName = string.Empty;
    private static string _gamificationUndolikeSentSamePersonTopicName = string.Empty;

    public KudosMessageSender(IMessageSender queue, IConfiguration configuration)
    {
        _connectionString = configuration["KudosServiceBus_ConnectionString"];

        _messageSender = queue;

        _notificationTopicName = configuration["KudosServiceBus_TopicName"];

        ReadConfigurationSettings(configuration);


        _messageSender.CreateQueueIfNotExistsAsync(_gamificationKudosSentTopicName).ConfigureAwait(false);
        _messageSender.CreateQueueIfNotExistsAsync(_gamificationKudosReceivedTopicName).ConfigureAwait(false);

        _messageSender.CreateQueueIfNotExistsAsync(_gamificationLikeSentTopicName).ConfigureAwait(false);
        _messageSender.CreateQueueIfNotExistsAsync(_gamificationLikeReceivedTopicName).ConfigureAwait(false);

        _messageSender.CreateQueueIfNotExistsAsync(_gamificationUndolikeSentTopicName).ConfigureAwait(false);
        _messageSender.CreateQueueIfNotExistsAsync(_gamificationUndolikeReceivedTopicName).ConfigureAwait(false);

        _messageSender.CreateTopicIfNotExistsAsync(_kudosSentDashboard).ConfigureAwait(false);
        _messageSender.CreateTopicIfNotExistsAsync(_likeSentDashboard).ConfigureAwait(false);
        _messageSender.CreateTopicIfNotExistsAsync(_likeUndoDashboard).ConfigureAwait(false);

        _messageSender.CreateQueueIfNotExistsAsync(_gamificationLikeSentSamePersonTopicName).ConfigureAwait(false);

        _messageSender.CreateQueueIfNotExistsAsync(_gamificationUndolikeSentSamePersonTopicName).ConfigureAwait(false);
    }

    private static void ReadConfigurationSettings(IConfiguration configuration)
    {
        _gamificationKudosSentTopicName = configuration["KudosServiceBus_GamificationKudosSentTopicName"];
        _gamificationKudosReceivedTopicName = configuration["KudosServiceBus_GamificationKudosReceivedTopicName"];

        _gamificationLikeSentTopicName = configuration["KudosServiceBus_GamificationLikeSentTopicName"];
        _gamificationLikeReceivedTopicName = configuration["KudosServiceBus_GamificationLikeReceivedTopicName"];
        
        _gamificationLikeSentSamePersonTopicName = configuration["KudosServiceBus_GamificationLikeSentSamePersonTopicName"];
        _gamificationUndolikeSentSamePersonTopicName = configuration["KudosServiceBus_GamificationUndolikeSentSamePersonTopicName"];

        _gamificationUndolikeSentTopicName = configuration["KudosServiceBus_GamificationUndolikeSentTopicName"];
        _gamificationUndolikeReceivedTopicName = configuration["KudosServiceBus_GamificationUndolikeReceivedTopicName"];

        _likeSentDashboard = configuration["KudosServiceBus_LikeSentDashboard"];
        _likeUndoDashboard = configuration["KudosServiceBus_LikeUndoDashboard"];

        _kudosSentDashboard = configuration["KudosServiceBus_KudosSentDashboard"];

        _notificationTopicName = configuration["KudosServiceBus_AgentTopicName"];
    }


    public async Task SendKudosAsync(string kudosId, KudosNotification kudos)
    {

        //send notification via Bot
        await _messageSender.SendQueue(kudos, _notificationTopicName);

        //gamification
        await _messageSender.SendQueue(kudos.From.Id, _gamificationKudosSentTopicName);
        await _messageSender.SendQueue(kudos.To.Id, _gamificationKudosReceivedTopicName);

        //notification to update the Teams Apps
        await _messageSender.SendTopic(
            new KudosResponse
            {
                Id = kudosId,
                To = kudos.To,
                From = kudos.From,
                Message = kudos.Message,
                Title = kudos.Reward.Title,
                SendOn = kudos.SendOn,
                Comments = new(),
                Likes = new List<Person>()
            },
            _kudosSentDashboard, _kudosSentDashboard);
    }


    public async Task SendLikeAsync(LikeGateway like)
    {
        var serviceBusAdminClient = new ServiceBusAdministrationClient(_connectionString);

        //gamification

        if (like.ToPersonId == like.FromPerson.Id)
        {
            await _messageSender.SendQueue(like.FromPerson.Id, _gamificationLikeSentSamePersonTopicName);
        }
        else
        {

            await _messageSender.SendQueue(like.FromPerson.Id, _gamificationLikeSentTopicName);
            await _messageSender.SendQueue(like.ToPersonId, _gamificationLikeReceivedTopicName);

        }
        //notification to update the Teams Apps
        await _messageSender.SendTopic(like, _likeSentDashboard, _likeSentDashboard);
    }

    public async Task SendUndoLikeAsync(LikeGateway like)
    {
        var serviceBusAdminClient = new ServiceBusAdministrationClient(_connectionString);

        //gamification
        if (like.ToPersonId == like.FromPerson.Id)
        {
            await _messageSender.SendQueue(like.FromPerson.Id, _gamificationUndolikeSentSamePersonTopicName);
        }
        else
        {
            await _messageSender.SendQueue(like.FromPerson.Id, _gamificationUndolikeSentTopicName);
            await _messageSender.SendQueue(like.ToPersonId, _gamificationUndolikeReceivedTopicName);
        }
        //notification to update the Teams Apps
        await _messageSender.SendTopic(like, _likeUndoDashboard, _likeUndoDashboard);
    }


}
