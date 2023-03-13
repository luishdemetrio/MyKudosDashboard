using Azure.Messaging.ServiceBus;
using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;
using Newtonsoft.Json;


namespace MyKudosDashboard.Views;

public class KudosListView : IKudosListView
{

    private ServiceBusClient _serviceBusClient;
    private ServiceBusProcessor _serviceBusLikeProcessor;
    private ServiceBusProcessor _serviceBusKudosProcessor;

    private readonly IGatewayService _gatewayService;

    public IKudosListView.UpdateLikeCallBack LikeCallback { get; set; }

    public IKudosListView.UpdateKudosCallBack KudosCallback { get; set; }


    public KudosListView(IGatewayService gatewayService)
    {
        _gatewayService = gatewayService;

        var connectionString = "Endpoint=sb://virtualkudosmessaging.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ygd5/DAwj+Rx3Lr5SGjjqZsq1dkUj/6pV+ASbErqEa8=";

        _serviceBusClient = new ServiceBusClient(connectionString);

        ServiceBusLikeMessageProcessor();

        ServiceBusKudosMessageProcessor();
    }

    public async Task<bool> SendLikeAsync(Like like)
    {
        return await _gatewayService.SendLike(like);
    }

    private async void ServiceBusLikeMessageProcessor()
    {

       
               
        //create a topic if it doesnt exist
        //var serviceBusAdminClient = new ServiceBusAdministrationClient(connectionString);

        //if (!await serviceBusAdminClient.TopicExistsAsync("dashboard"))
        //{
        //    await serviceBusAdminClient.CreateTopicAsync("dashboard");
        //}

        ////create a temp subscription for the user

        //if (!await serviceBusAdminClient.SubscriptionExistsAsync("dashboard", "notification"))
        //{
        //    var options = new CreateSubscriptionOptions("dashboard", "notification")
        //    {
        //        AutoDeleteOnIdle = TimeSpan.FromHours(1)
        //    };

        //    await serviceBusAdminClient.CreateSubscriptionAsync(options);
        //}

        

        _serviceBusLikeProcessor = _serviceBusClient.CreateProcessor("dashboard", "notification");

        _serviceBusLikeProcessor.ProcessMessageAsync += ServiceBusLikeProcessor_ProcessMessageAsync;

        _serviceBusLikeProcessor.ProcessErrorAsync += ServiceBusProcessor_ProcessErrorAsync;

        await _serviceBusLikeProcessor.StartProcessingAsync();
    }

    private async void ServiceBusKudosMessageProcessor()
    {

        _serviceBusKudosProcessor = _serviceBusClient.CreateProcessor("kudosdashboard", "notification");

        _serviceBusKudosProcessor.ProcessMessageAsync += ServiceBusKudosProcessor_ProcessMessageAsync;
        _serviceBusKudosProcessor.ProcessErrorAsync += ServiceBusProcessor_ProcessErrorAsync;

        await _serviceBusKudosProcessor.StartProcessingAsync();
    }


    private async Task ServiceBusProcessor_ProcessErrorAsync(ProcessErrorEventArgs arg)
    {
       
    }

    private async Task ServiceBusLikeProcessor_ProcessMessageAsync(ProcessMessageEventArgs arg)
    {
        //retrive the message body

        var like = JsonConvert.DeserializeObject<Like>(arg.Message.Body.ToString());

        if (like != null)
        {
            LikeCallback?.Invoke(like);
        }

        await arg.CompleteMessageAsync(arg.Message);
    }

    private async Task ServiceBusKudosProcessor_ProcessMessageAsync(ProcessMessageEventArgs arg)
    { 
        //retrive the message body

        var kudos = JsonConvert.DeserializeObject<KudosResponse>(arg.Message.Body.ToString());

        if (kudos != null)
        {
            KudosCallback?.Invoke(kudos);
        }

        await arg.CompleteMessageAsync(arg.Message);
    }
}
