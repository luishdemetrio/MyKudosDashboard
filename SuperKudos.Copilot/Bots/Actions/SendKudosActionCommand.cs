using Azure.Identity;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using MyKudos.Communication.Helper.Interfaces;

namespace SuperKudos.Copilot.Bots;

public class SendKudosActionCommand : IActionCommand
{
    private IRestClientHelper _restClientHelper;
    private string _gatewayServiceUrl;

    private ClientSecretCredential _clientSecretCredential;
    private GraphServiceClient _appClient;

    private string _sendKudosPage;

    public SendKudosActionCommand(IRestClientHelper clientHelper, IConfiguration configuration)
    {
        _restClientHelper = clientHelper;
        _gatewayServiceUrl = configuration["GatewayServiceUrl"];

        _sendKudosPage = configuration["SendKudosPage"];


        _clientSecretCredential = new ClientSecretCredential(
              configuration["tenantId"], configuration["BOT_ID"], configuration["BOT_PASSWORD"]);


        _appClient = new GraphServiceClient(_clientSecretCredential,
            // Use the default scope, which will request the scopes
            // configured on the app registration
            new[] { "https://graph.microsoft.com/.default" });
    }

    public async Task<AdaptiveCardInvokeResponse> Execute(ITurnContext<IInvokeActivity> turnContext,
                                                    AdaptiveCardInvokeValue invokeValue,
                                                    CancellationToken cancellationToken)
    {
       
        string toPersonId = string.Empty;
        //string toPersonName = string.Empty;

        string fromPersonId = string.Empty;
        //string fromPersonName = string.Empty;

        

        return null;
    }
}
