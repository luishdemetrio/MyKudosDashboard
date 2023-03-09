using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Views;

public class KudosListView : IKudosListView
{

    private ServiceBusClient _serviceBusClient;
    private ServiceBusProcessor _serviceBusProcessor;

    private readonly IGatewayService _gatewayService;

    public KudosListView(IGatewayService gatewayService)
    {
        _gatewayService = gatewayService;

        ServiceBusMessageProcessor();
    }

    public async Task<bool> SendLikeAsync(Like like)
    {
        return await _gatewayService.SendLike(like);
    }

    private async void ServiceBusMessageProcessor()
    {

        var connectionString = "Endpoint=sb://virtualkudosmessaging.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ygd5/DAwj+Rx3Lr5SGjjqZsq1dkUj/6pV+ASbErqEa8=";

       


        //create a topic if it doesnt exist
        var serviceBusAdminClient = new ServiceBusAdministrationClient(connectionString);

        if (!await serviceBusAdminClient.TopicExistsAsync("dashboard"))
        {
            await serviceBusAdminClient.CreateTopicAsync("dashboard");
        }

        //create a temp subscription for the user

        if (!await serviceBusAdminClient.SubscriptionExistsAsync("dashboard", "notification"))
        {
            var options = new CreateSubscriptionOptions("dashboard", "notification")
            {
                AutoDeleteOnIdle = TimeSpan.FromHours(1)
            };

            await serviceBusAdminClient.CreateSubscriptionAsync(options);
        }

        _serviceBusClient = new ServiceBusClient(connectionString);
        
        _serviceBusProcessor = _serviceBusClient.CreateProcessor("dashboard", "notification");

        _serviceBusProcessor.ProcessMessageAsync += ServiceBusProcessor_ProcessMessageAsync;

        _serviceBusProcessor.ProcessErrorAsync += ServiceBusProcessor_ProcessErrorAsync;

        await _serviceBusProcessor.StartProcessingAsync();
    }

    private Task ServiceBusProcessor_ProcessErrorAsync(ProcessErrorEventArgs arg)
    {
        return Task.CompletedTask;
    }

    private async Task ServiceBusProcessor_ProcessMessageAsync(ProcessMessageEventArgs arg)
    {
       //retrive the message body

        var like = arg.Message.Body;

        await arg.CompleteMessageAsync(arg.Message);
    }
}
