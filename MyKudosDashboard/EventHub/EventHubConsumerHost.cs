



using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;

using System.Text;
using System.Text.Json;

namespace MyKudosDashboard.EventHub;



public class EventHubConsumerHelper<T>  
{

    string _blobStorageConnectionString ;
    string _blobContainerName ;
    string _consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

    EventProcessorClient processor;
    CancellationTokenSource cancellationSource;

    public delegate void NotifyCallBack(T json);

    public NotifyCallBack UpdateCallback { get; set; }

    public EventHubConsumerHelper(string connectionString, string eventHubName,
                                  string blobStorageConnectionString, string blobContainerName)
    {
        cancellationSource = new CancellationTokenSource();
        cancellationSource.CancelAfter(TimeSpan.FromSeconds(45));


        _blobStorageConnectionString = blobStorageConnectionString;
        _blobContainerName = blobContainerName;

        var eventProcessorHostName = Guid.NewGuid().ToString();

        var storageClient = new BlobContainerClient(blobStorageConnectionString, blobContainerName);

         processor = new EventProcessorClient(storageClient,
            _consumerGroup,
            connectionString,
            eventHubName);


        processor.ProcessEventAsync += processEventHandler;
        processor.ProcessErrorAsync += processErrorHandler;

        
    }

   
    public async Task Start()
    {

        try
        {
            await processor.StartProcessingAsync();

        }catch(Exception ex)
        {

        }

    }

    public async Task Stop()
    {

        try
        {
            await processor.StopProcessingAsync();

        }
        catch (Exception ex)
        {

        }


    }

    async Task processEventHandler(ProcessEventArgs eventArgs)
        {
            try
            {
            
            var t = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(eventArgs.Data.EventBody.ToArray()));
            

            UpdateCallback?.Invoke(t);

            await eventArgs.UpdateCheckpointAsync();

                // Perform the application-specific processing for an event.  This method
                // is intended for illustration and is not defined in this snippet.

               // await DoSomethingWithTheEvent(eventArgs.Partition, eventArgs.Data);
            }
            catch
            {
                // Handle the exception from handler code
            }
        }

        async Task processErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            try
            {
                // Perform the application-specific processing for an error.  This method
                // is intended for illustration and is not defined in this snippet.

              //  await DoSomethingWithTheError(eventArgs.Exception);
            }
            catch
            {
                // Handle the exception from handler code
            }
        }

        



    


}

