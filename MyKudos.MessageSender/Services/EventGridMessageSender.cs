using Azure;
using Azure.Messaging.EventGrid;
using Newtonsoft.Json;

namespace MyKudos.MessageSender.Services;

public class EventGridMessageSender 
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

    public async Task SendTopic(object topicMessage,  string subject, string eventType)
    {
        var eventGridEvent = new EventGridEvent(
            subject: subject,
            data: JsonConvert.SerializeObject(topicMessage),
            eventType: eventType,
            dataVersion: "1.0"
        );

        List<EventGridEvent> eventList = new List<EventGridEvent> { eventGridEvent };

        await _eventGridPublisherClient.SendEventsAsync(eventList);
    }
}

