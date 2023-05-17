using Microsoft.Extensions.Configuration;
using MyKudos.Gamification.Domain.Models;
using MyKudos.Gamification.Receiver.Interfaces;
using MyKudos.MessageSender.Interfaces;
using System.Threading.Tasks;

namespace MyKudos.Gamification.Receiver.MessageSenders;

public class ScoreMessageSender : IScoreMessageSender
{

    private IMessageSender _messageSender;

    private string _topicNameScoreUpdated = string.Empty;

    private string _topicNameScoreUpdatedSamePerson = string.Empty;

    public ScoreMessageSender(IMessageSender queue, IConfiguration configuration)
    {
        _messageSender = queue;

        ReadConfigurationSettings(configuration);

        _messageSender.CreateTopicIfNotExistsAsync(_topicNameScoreUpdated).ConfigureAwait(false);

        _messageSender.CreateTopicIfNotExistsAsync(_topicNameScoreUpdatedSamePerson).ConfigureAwait(false);

    }

    private void ReadConfigurationSettings(IConfiguration configuration)
    {
        _topicNameScoreUpdated = configuration["KudosServiceBus_ScoreUpdatedDashboard"];

        _topicNameScoreUpdatedSamePerson = configuration["KudosServiceBus_ScoreUpdatedSamePersonDashboard"];

    }

    public async Task NotifyProfileScoreUpdated(UserScore score)
    {
        await _messageSender.SendTopic(score, _topicNameScoreUpdated, "ProfileUpdated");
    }

    public async Task NotifyProfileScoreSamePersonUpdated(UserScore score)
    {
        await _messageSender.SendTopic(score, _topicNameScoreUpdatedSamePerson, "ProfileSamePersonUpdated");
    }
}

