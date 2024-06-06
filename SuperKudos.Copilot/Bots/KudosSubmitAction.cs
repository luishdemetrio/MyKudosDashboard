using AdaptiveCards;
using Azure.Identity;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Graph;
using Microsoft.Graph.Security.Triggers.RetentionEvents.Item.RetentionEventType;
using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gateway.Domain.Models;
using Newtonsoft.Json.Linq;

namespace SuperKudos.Copilot.Bots;



public class KudosSubmitAction : ISubmitAction
{
    private IRestClientHelper _restClientHelper;
    private string _gatewayServiceUrl;
    private readonly string _adaptiveCardKudosSentFilePath = Path.Combine(".", "Resources", "KudosSent.json");

   


    public KudosSubmitAction(IRestClientHelper clientHelper, IConfiguration configuration)
    {
        _restClientHelper = clientHelper;
        _gatewayServiceUrl = configuration["GatewayServiceUrl"];

       
    }

    public async Task<MessagingExtensionActionResponse> SubmitAction(ITurnContext<IInvokeActivity> turnContext,
                                                                              MessagingExtensionAction action)
    {

        // Get the data object from the invokeValue
        JObject data = (JObject)action.Data;

        var kudos = new SendKudosRequest {
            FromPersonId = Guid.Parse(data.Value<string>("fromPersonId")),
            Message = data.Value<string>("messageInput"),
            RecognitionId = data.Value<int>("selectedBehavior"),
            Date = DateTime.Now,
            ToPersonId = new List<Guid>() { Guid.Parse(data.Value<string>("toPersonId"))}
        };

        var kudosId = SendKudos(kudos);

        // The kudos card will be sent via Super Kudos App
        return null;

        

        var templateJson = await System.IO.File.ReadAllTextAsync(_adaptiveCardKudosSentFilePath);

        var template = new AdaptiveCards.Templating.AdaptiveCardTemplate(templateJson);

        var adaptiveCardJson = template.Expand(new { ToPerson = "Pessoa" });


        var adaptiveCard = AdaptiveCard.FromJson(adaptiveCardJson).Card;
        var attachments = new MessagingExtensionAttachment()
        {
            ContentType = AdaptiveCard.ContentType,
            Content = adaptiveCard
        };

        return new MessagingExtensionActionResponse
        {
            ComposeExtension = new MessagingExtensionResult
            {
                Type = "result",
                AttachmentLayout = "list",
                Attachments = new[] { attachments }
            }
        };

    }

    private async Task<string> SendKudos(SendKudosRequest kudos)
    {

        return await _restClientHelper.SendApiData<SendKudosRequest, string>($"{_gatewayServiceUrl}kudos", HttpMethod.Post, kudos);

    }


}
