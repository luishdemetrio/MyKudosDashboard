using Azure;
using Azure.Messaging.EventGrid;
using Azure.Messaging.ServiceBus;
using MyKudos.MessageSender.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyKudos.MessageSender.Services;

public class EventGridMessageSender : IMessageSender
{
    private  string _topicEndpoint;
    private  string _topicKey;

    private EventGridPublisherClient _eventGridPublisherClient;

    public EventGridMessageSender(string topicEndpoint, string topicKey)
    {
        _topicEndpoint = topicEndpoint;
        _topicKey = topicKey;

        _eventGridPublisherClient = new EventGridPublisherClient(new Uri(_topicEndpoint), new AzureKeyCredential(_topicKey));
    }

    public async Task CreateTopicIfNotExistsAsync(string topicName)
    {
        // Azure Event Grid topics are created as separate resources in the Azure portal or using Azure CLI/PowerShell.
        // There is no direct method to create a topic using the EventGridPublisherClient.
        // You can create a topic using Azure Management SDKs or Azure Resource Manager templates.
    }

    public async Task CreateQueueIfNotExistsAsync(string queueName)
    {
        // Azure Event Grid does not have the concept of queues. It uses topics and subscriptions for event routing.
    }

    public async Task SendQueue(object queueMessage, string queueName)
    {
        // Azure Event Grid does not have the concept of queues. It uses topics and subscriptions for event routing.
    }

    public async Task SendTopic(object topicMessage, string topic, string subject)
    {
        var eventGridEvent = new EventGridEvent(
            subject: subject,
            data: JsonConvert.SerializeObject(topicMessage),
            eventType: "CustomEventType",
            dataVersion: "1.0"
        );

        List<EventGridEvent> eventList = new List<EventGridEvent> { eventGridEvent };

        await _eventGridPublisherClient.SendEventsAsync(eventList);
    }
}

