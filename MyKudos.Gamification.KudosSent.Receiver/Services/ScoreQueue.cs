using Azure.Messaging.ServiceBus.Administration;
using Azure.Messaging.ServiceBus;
using MyKudos.Gamification.Domain.Models;
using MyKudos.Gamification.Receiver.Interfaces;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace MyKudos.Gamification.Receiver.Services;

public class ScoreQueue : IScoreQueue
{

    private static string _connectionString = string.Empty;

    public ScoreQueue(IConfiguration configuration)
    {
        _connectionString = configuration["KudosServiceBus_ConnectionString"];

    }

    private static async Task SendTopic(object queueMessage, ServiceBusAdministrationClient serviceBusAdminClient,
                                      string topic, string subscriptionName)
    {

        //create a topic if it doesnt exist

        if (!await serviceBusAdminClient.TopicExistsAsync(topic))
        {
            await serviceBusAdminClient.CreateTopicAsync(topic);
        }

        //create a temp subscription for the user

        if (!await serviceBusAdminClient.SubscriptionExistsAsync(topic, subscriptionName))
        {
            var options = new CreateSubscriptionOptions(topic, subscriptionName)
            {
                AutoDeleteOnIdle = TimeSpan.FromHours(1)
            };

            await serviceBusAdminClient.CreateSubscriptionAsync(options);
        }

        var client = new ServiceBusClient(_connectionString);

        var sender = client.CreateSender(topic);

        var message = new ServiceBusMessage(JsonConvert.SerializeObject(queueMessage));

        await sender.SendMessageAsync(message);

        await sender.CloseAsync();
    }

    public async Task NotifyProfileScoreUpdated(UserScore score)
    {
        
        var serviceBusAdminClient = new ServiceBusAdministrationClient(_connectionString);

        await SendTopic(score, serviceBusAdminClient, "dashboardscoreupdated", score.UserId);
    }
}
