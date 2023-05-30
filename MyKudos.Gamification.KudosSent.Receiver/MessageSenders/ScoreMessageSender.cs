using Microsoft.Extensions.Configuration;
using MyKudos.Kudos.Domain.Models;
using MyKudos.Gamification.Receiver.Interfaces;
using MyKudos.MessageSender.Interfaces;
using System.Threading.Tasks;

namespace MyKudos.Gamification.Receiver.MessageSenders;

public class ScoreMessageSender : IScoreMessageSender
{

    private IMessageSender _messageSender;

    private string _topicNameScoreUpdated = string.Empty;

    

    public ScoreMessageSender(IMessageSender queue, IConfiguration configuration)
    {
        _messageSender = queue;

        ReadConfigurationSettings(configuration);

        _messageSender.CreateTopicIfNotExistsAsync(_topicNameScoreUpdated).ConfigureAwait(false);

      

    }

    private void ReadConfigurationSettings(IConfiguration configuration)
    {
        _topicNameScoreUpdated = configuration["KudosServiceBus_ScoreUpdatedDashboard"];

      

    }

    public async Task NotifyProfileScoreUpdated(UserPointScore score)
    {
        await _messageSender.SendTopic(score, _topicNameScoreUpdated, "ProfileUpdated");
    }

    
}

