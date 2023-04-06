using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;
using Azure.Messaging.ServiceBus.Administration;
using Newtonsoft.Json;
using Azure.Messaging.ServiceBus;

namespace MyKudosDashboard.Views;

public class KudosTabView : IKudosTabView
{
    private IGatewayService _gatewayService;

    private ServiceBusClient _serviceBusClient;
    private ServiceBusProcessor _serviceBusLikeProcessor;
    private ServiceBusProcessor _serviceBusKudosProcessor;

    private string _serviceBusConnectionString;

    public IKudosTabView.UpdateLikesCallBack LikeCallback { get; set; }

    public IKudosTabView.UpdateKudosCallBack KudosCallback { get; set; }

    public KudosTabView(IGatewayService gatewayService, IConfiguration configuration)
    {
        _gatewayService = gatewayService;

        _serviceBusConnectionString = configuration["KudosServiceBus_ConnectionString"];

        _serviceBusClient = new ServiceBusClient(_serviceBusConnectionString);
    }


    public async Task<IEnumerable<KudosResponse>> GetKudos()
    {
        return await _gatewayService.GetKudos();
    }


    private async Task CreateATopicIfItDoesntExistAsync(string topicName, string subscriptionName)
    {
        //create a topic if it doesnt exist
        var serviceBusAdminClient = new ServiceBusAdministrationClient(_serviceBusConnectionString);


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

    private void ServiceBusLikeMessageProcessor(string subscriptionName)
    {

        CreateATopicIfItDoesntExistAsync("dashboard", subscriptionName).ContinueWith( t =>
        {
            
            _serviceBusLikeProcessor = _serviceBusClient.CreateProcessor("dashboard", subscriptionName);

            _serviceBusLikeProcessor.ProcessMessageAsync += ServiceBusLikeProcessor_ProcessMessageAsync;

            _serviceBusLikeProcessor.ProcessErrorAsync += ServiceBusProcessor_ProcessErrorAsync;

            _serviceBusLikeProcessor.StartProcessingAsync();
            
        });


    }

    private void ServiceBusKudosMessageProcessor(string subscriptionName)
    {

        CreateATopicIfItDoesntExistAsync("kudosdashboard", subscriptionName).ContinueWith(t =>
        {

            _serviceBusKudosProcessor = _serviceBusClient.CreateProcessor("kudosdashboard", subscriptionName);

            _serviceBusKudosProcessor.ProcessMessageAsync += ServiceBusKudosProcessor_ProcessMessageAsync;
            _serviceBusKudosProcessor.ProcessErrorAsync += ServiceBusProcessor_ProcessErrorAsync;

            _serviceBusKudosProcessor.StartProcessingAsync();

        });
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
