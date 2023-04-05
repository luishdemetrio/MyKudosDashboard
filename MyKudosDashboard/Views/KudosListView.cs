using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
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

    private string _serviceBusConnectionString;


    public KudosListView(IGatewayService gatewayService, IConfiguration configuration)
    {
        _gatewayService = gatewayService;

        _serviceBusConnectionString = configuration["KudosServiceBus_ConnectionString"];

        _serviceBusClient = new ServiceBusClient(_serviceBusConnectionString);

    }

    public async Task<bool> SendLikeAsync(Like like)
    {
        return await _gatewayService.SendLike(like);
    }


    private async Task CreateATopicIfItDoesntExistAsync(string topicName, string subscriptionName)
    {
        //create a topic if it doesnt exist
        var serviceBusAdminClient = new ServiceBusAdministrationClient(_serviceBusConnectionString);

        if (!await serviceBusAdminClient.TopicExistsAsync(topicName))
        {
            await serviceBusAdminClient.CreateTopicAsync(topicName);
        }

        //create a temp subscription for the user

        if (!await serviceBusAdminClient.SubscriptionExistsAsync(topicName, subscriptionName))
        {
            var options = new CreateSubscriptionOptions(topicName, subscriptionName)
            {
                AutoDeleteOnIdle = TimeSpan.FromHours(1)
            };

            await serviceBusAdminClient.CreateSubscriptionAsync(options);
        }
    }

    private async void ServiceBusLikeMessageProcessor(string subscriptionName)
    {


        await CreateATopicIfItDoesntExistAsync("dashboard", subscriptionName);
                

        _serviceBusLikeProcessor = _serviceBusClient.CreateProcessor("dashboard", subscriptionName);

        _serviceBusLikeProcessor.ProcessMessageAsync += ServiceBusLikeProcessor_ProcessMessageAsync;

        _serviceBusLikeProcessor.ProcessErrorAsync += ServiceBusProcessor_ProcessErrorAsync;

        await _serviceBusLikeProcessor.StartProcessingAsync();
    }

    private async void ServiceBusKudosMessageProcessor(string subscriptionName)
    {

        await CreateATopicIfItDoesntExistAsync("kudosdashboard", subscriptionName);

        _serviceBusKudosProcessor = _serviceBusClient.CreateProcessor("kudosdashboard", subscriptionName);

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
        
        var kudos = JsonConvert.DeserializeObject<KudosResponse>(arg.Message.Body.ToString());
        
        if (kudos != null)
        {
            KudosCallback?.Invoke(kudos);
        }

        await arg.CompleteMessageAsync(arg.Message);
    }

    public void RegisterForLiveUpdates(string userId)
    {
        ServiceBusLikeMessageProcessor(userId);

        ServiceBusKudosMessageProcessor(userId);
    }
}
