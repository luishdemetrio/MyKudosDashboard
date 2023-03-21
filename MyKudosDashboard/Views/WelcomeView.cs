using Azure.Messaging.ServiceBus;
using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;
using Newtonsoft.Json;

namespace MyKudosDashboard.Views;

public class WelcomeView : IWelcomeView
{

    private ServiceBusClient _serviceBusClient;
    private ServiceBusProcessor _serviceBusScoreProcessor;

    private string _userId;

    private IGatewayService _gatewayService;

    public WelcomeView(IGatewayService gatewayService, IConfiguration configuration)
    {
        _gatewayService = gatewayService;

        _serviceBusClient = new ServiceBusClient(configuration["KudosServiceBus_ConnectionString"]);

    }

    public IWelcomeView.UpdateScoreCallBack ScoreCallback { get ; set ; }

    public async Task<string> GetUserPhoto(string userId)
    {

        return await _gatewayService.GetUserPhoto(userId);
    }

    public void RegisterForUserScoreUpdate(string userId)
    {
        
        _userId = userId;

        ServiceBusScoreProcessor(userId);

    }

    private async void ServiceBusScoreProcessor(string userId)
    {
        _serviceBusScoreProcessor = _serviceBusClient.CreateProcessor("dashboardscoreupdated", userId);

        _serviceBusScoreProcessor.ProcessMessageAsync += _serviceBusScoreProcessor_ProcessMessageAsync;

        _serviceBusScoreProcessor.ProcessErrorAsync += _serviceBusScoreProcessor_ProcessErrorAsync;

        await _serviceBusScoreProcessor.StartProcessingAsync();
    }

    private async Task _serviceBusScoreProcessor_ProcessErrorAsync(ProcessErrorEventArgs arg)
    {
        
    }

    private async Task _serviceBusScoreProcessor_ProcessMessageAsync(ProcessMessageEventArgs arg)
    {
        //retrive the message body

        var score = JsonConvert.DeserializeObject<UserScore>(arg.Message.Body.ToString());

        if ((score != null) && score.UserId == _userId )
        {
            ScoreCallback?.Invoke(score);
        }

        await arg.CompleteMessageAsync(arg.Message);
    }
}
