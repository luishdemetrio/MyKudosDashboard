
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using MyKudos.MessageSender.Interfaces;
using Newtonsoft.Json;

namespace MyKudos.Queue.Services;

public class ServiceBusMessageSender : IMessageSender
{

    private static string _connectionString = string.Empty;
    
    private ServiceBusClient _serviceBusClient;

    public ServiceBusMessageSender(string serviceBusConnectionString)
    {
        _connectionString = serviceBusConnectionString;

        _serviceBusClient = new ServiceBusClient(_connectionString);
    }

    public async Task CreateTopicIfNotExistsAsync(string topicName)
    {
        var serviceBusAdminClient = new ServiceBusAdministrationClient(_connectionString);

        if (!await serviceBusAdminClient.TopicExistsAsync(topicName))
        {
            await serviceBusAdminClient.CreateTopicAsync(topicName);
        }
    }

    public async Task CreateQueueIfNotExistsAsync(string topicName)
    {
        var serviceBusAdminClient = new ServiceBusAdministrationClient(_connectionString);

        if (!await serviceBusAdminClient.QueueExistsAsync(topicName))
        {
            await serviceBusAdminClient.CreateQueueAsync(topicName);
        }
    }

    public async Task SendQueue(object queueMessage, string queueName)
    {
        var client = new ServiceBusClient(_connectionString);

        var sender = client.CreateSender(queueName);

        var message = new ServiceBusMessage(JsonConvert.SerializeObject(queueMessage))
        {
            Subject = queueName,
            ContentType = "application/json"
        };

        await sender.SendMessageAsync(message);

        await sender.CloseAsync();
    }

    public async Task SendTopic(object queueMessage, string topic, string subject)
    {
        var sender = _serviceBusClient.CreateSender(topic);

        var message = new ServiceBusMessage(JsonConvert.SerializeObject(queueMessage))
        {
            Subject = subject,
            ContentType = "application/json"
        };

        await sender.SendMessageAsync(message);

        await sender.CloseAsync();
    }
}
