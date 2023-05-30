using MyKudos.Gateway.Interfaces;
using MyKudos.Gateway.Domain.Models;
using MyKudos.MessageSender.Interfaces;

namespace MyKudos.Gateway.Queues;

public class CommentsMessageSender : ICommentsMessageSender
{

    private IMessageSender _messageSender;

    //private static string _gamificationMessageSent = string.Empty;
    //private static string _gamificationMessageReceived = string.Empty;

    //private static string _gamificationMessageDeletedFromTopicName = string.Empty;
    //private static string _gamificationMessageDeletedToTopicName = string.Empty;

    private static string _messageSentDashboard = string.Empty;
    private static string _messageDeletedDashboard = string.Empty;
    private static string _messageUpdatedDashboard = string.Empty;

    private static string _notifyUserPoints = string.Empty;

    public CommentsMessageSender(IMessageSender queue, IConfiguration configuration)
    {
        _messageSender = queue;

        ReadConfigurationSettings(configuration);

        _messageSender.CreateTopicIfNotExistsAsync(_messageSentDashboard).ConfigureAwait(false);
        _messageSender.CreateTopicIfNotExistsAsync(_messageDeletedDashboard).ConfigureAwait(false);
        _messageSender.CreateTopicIfNotExistsAsync(_messageUpdatedDashboard).ConfigureAwait(false);

        _messageSender.CreateQueueIfNotExistsAsync(_notifyUserPoints).ConfigureAwait(false);
    }

    private static void ReadConfigurationSettings(IConfiguration configuration)
    {
       
        _messageSentDashboard = configuration["KudosServiceBus_MessageSentDashboard"];
        _messageDeletedDashboard = configuration["KudosServiceBus_MessageDeletedDashboard"];
        _messageUpdatedDashboard = configuration["KudosServiceBus_MessageUpdatedDashboard"];

        _notifyUserPoints = configuration["KudosServiceBus_NotifyUserPoints"];

    }


    private async Task NotifyUserScore(CommentsRequest comments)
    {
        //notify User Points
        await _messageSender.SendQueue(comments.FromPersonId, _notifyUserPoints);


        if (comments.ToPersonId != comments.FromPersonId)
        {
            //the equality can happens when the person who received the kudos comments on his/her kudos to thanks
            //in this case we dont need to notify it again
            await _messageSender.SendQueue(comments.ToPersonId, _notifyUserPoints);
        }
    }
    public async Task MessageSent(CommentsRequest comments)
    {
        //notify User Points
        await NotifyUserScore(comments);

        //notification to update the Teams Apps
        await _messageSender.SendTopic(comments, _messageSentDashboard, "MessageSent");
    }

    public async Task MessageDeleted(CommentsRequest comments)
    {
        //notify User Points
        await NotifyUserScore(comments);

        //notification to update the Teams Apps
        await _messageSender.SendTopic(comments, _messageDeletedDashboard, "MessageDeleted");
    }

    public async Task MessageUpdated(CommentsRequest comments)
    {
        //notification to update the Teams Apps
        await _messageSender.SendTopic(comments, _messageUpdatedDashboard, "MessageUpdated");
    }
}
