using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System.Text.Json;

namespace MyKudos.MessageSender.Services;

public class EventHubMessageSender
{

    private readonly string _connectionString;
    private readonly string _eventHubName;

    
    public EventHubMessageSender(string connectionString, string eventHubName)
    {

        _connectionString = connectionString;
        _eventHubName = eventHubName;
        
    }


    public async Task PublishAsync<T> (T myEvent)
    {

        await using var producerClient =
                    new EventHubProducerClient(_connectionString, _eventHubName);

        using EventDataBatch eventBatch =
                        await producerClient.CreateBatchAsync();


        // serialize the event

        var eventBytes = JsonSerializer.SerializeToUtf8Bytes(myEvent);


        // wrap event bytes in EventData instance

        var eventData = new EventData(eventBytes);

        eventBatch.TryAdd(eventData);

        //publish the event
        await producerClient.SendAsync(eventBatch);
    }


}
