using Microsoft.Extensions.Configuration;
using MyKudos.Gamification.Domain.Models;
using MyKudos.Gamification.Receiver.Interfaces;
using MyKudos.MessageSender.Interfaces;
using System.Threading.Tasks;

namespace MyKudos.Gamification.Receiver.MessageSenders;

internal class ScoreMessageSender : IScoreMessageSender
{

    private IMessageSender _messageSender;

    private string _topicName = string.Empty;

    public ScoreMessageSender(IMessageSender queue, IConfiguration configuration)
    {
        _messageSender = queue;

        ReadConfigurationSettings(configuration);

        _messageSender.CreateTopicIfNotExistsAsync(_topicName).ConfigureAwait(false);

    }

    private void ReadConfigurationSettings(IConfiguration configuration)
    {
        _topicName = configuration["KudosServiceBus_ScoreUpdatedTopicName"];
       
    }

    public async Task NotifyProfileScoreUpdated(UserScore score)
    {
        await _messageSender.SendTopic(score, _topicName, "ProfileUpdated");
    }
}

