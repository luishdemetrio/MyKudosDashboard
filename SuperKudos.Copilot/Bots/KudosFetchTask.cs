using AdaptiveCards;
using Azure.Identity;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gateway.Domain.Models;

namespace SuperKudos.Copilot.Bots;

public interface IKudosFetchTask
{
    Task<MessagingExtensionActionResponse> SendKudosCreateFormCard(ITurnContext<IInvokeActivity> turnContext);

    Task<MessagingExtensionActionResponse> SendKudosEmbeddedWebView(ITurnContext<IInvokeActivity> turnContext);
}

public class KudosFetchTask : IKudosFetchTask
{
    private readonly string _adaptiveCardSendKudosFilePath = Path.Combine(".", "Resources", "SendKudosCard.json");

    private IRestClientHelper _restClientHelper;
    private string _gatewayServiceUrl;

    private ClientSecretCredential _clientSecretCredential;
    private GraphServiceClient _appClient;

    public KudosFetchTask(IRestClientHelper clientHelper, IConfiguration configuration)
    {
        _restClientHelper = clientHelper;
        _gatewayServiceUrl = configuration["GatewayServiceUrl"];

        _clientSecretCredential = new ClientSecretCredential(
              configuration["tenantId"], configuration["BOT_ID"], configuration["BOT_PASSWORD"]);


        _appClient = new GraphServiceClient(_clientSecretCredential,
            // Use the default scope, which will request the scopes
            // configured on the app registration
            new[] { "https://graph.microsoft.com/.default" });
    }


    public async Task<MessagingExtensionActionResponse> SendKudosEmbeddedWebView(ITurnContext<IInvokeActivity> turnContext)
    {

        // Retrieve chat members
        var chatMembers = await _appClient.Chats[turnContext.Activity.Conversation.Id].Members.GetAsync();

        string toPersonId = string.Empty;
        //string toPersonName = string.Empty;

        string fromPersonId = string.Empty;
        //string fromPersonName = string.Empty;

        foreach (AadUserConversationMember member in chatMembers.Value)
        {
            if (member.UserId == turnContext.Activity.From.AadObjectId)
            {
                fromPersonId = member.UserId;
          //      fromPersonName = member.DisplayName;
            }
            else
            {
                toPersonId = member.UserId;
            //    toPersonName = member.DisplayName;
            }
        }


        var response = new MessagingExtensionActionResponse
        {
            Task = new TaskModuleContinueResponse
            {
                Value = new TaskModuleTaskInfo
                {
                    Height = "large",
                    Width = "large",
                    Title = "Super Kudos",
                    Url = $"https://localhost:44302/kudos?touserid={toPersonId}&fromuserid={fromPersonId}"
                }
            }
        };

        return response;
    }

public async Task<MessagingExtensionActionResponse> SendKudosCreateFormCard(ITurnContext<IInvokeActivity> turnContext)
    {

        // Retrieve chat members
        var chatMembers = await _appClient.Chats[turnContext.Activity.Conversation.Id].Members.GetAsync();

        string toPersonId = string.Empty;
        string toPersonName = string.Empty;

        string fromPersonId = string.Empty;
        string fromPersonName = string.Empty;

        foreach (AadUserConversationMember member in chatMembers.Value)
        {
            if (member.UserId == turnContext.Activity.From.AadObjectId)
            {
                fromPersonId = member.UserId;
                fromPersonName = member.DisplayName;
            }
            else
            {
                toPersonId = member.UserId;
                toPersonName = member.DisplayName;
            }
        }

        

        var templateJson = await System.IO.File.ReadAllTextAsync(_adaptiveCardSendKudosFilePath);

        var template = new AdaptiveCards.Templating.AdaptiveCardTemplate(templateJson);

        var recognitions = await GetRecognitions();
        var recognitionChoices = recognitions.Select(r => new { title = r.Title, 
                                                                value = r.RecognitionId,
                                                                
                                                              });

        var adaptiveCardJson = template.Expand(new {    selectedBehavior = recognitionChoices,
                                                        ToPersonId = toPersonId,
                                                        ToPersonName = toPersonName,
                                                        FromPersonId = fromPersonId,
                                                        FromPersonName = fromPersonName
                                                    });


        var adaptiveCard = AdaptiveCard.FromJson(adaptiveCardJson).Card;

        var attachments = new MessagingExtensionAttachment()
        {
            ContentType = AdaptiveCard.ContentType,
            Content = adaptiveCard
        };

        return new MessagingExtensionActionResponse
        {
            Task = new TaskModuleContinueResponse()
            {
                Value = new TaskModuleTaskInfo()
                {
                    Height = "medium",
                    Width = "medium",
                    Title = "Send a Super Kudos",
                    Card = attachments
                }
            }
        };
    }

    private async Task<IEnumerable<Recognition>> GetRecognitions()
    {

        return await _restClientHelper.GetApiData<IEnumerable<Recognition>>($"{_gatewayServiceUrl}Recognition");

    }



}
