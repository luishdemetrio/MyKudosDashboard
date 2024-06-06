using AdaptiveCards;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Graph.Models;
using MyKudos.Communication.Helper.Interfaces;
using SuperKudos.Copilot.Helpers;
using System.Diagnostics;
using static System.Collections.Specialized.BitVector32;

namespace SuperKudos.Copilot.Bots;

public class SendKudosMessageCommand : IMessageCommand
{
    private IRestClientHelper _restClientHelper;
    private string _gatewayServiceUrl;

    private string _sendKudosPage;

    private readonly string _adaptiveCardFilePath = Path.Combine(".", "Resources", "KudosPreviewMessage.json");

    public SendKudosMessageCommand(IRestClientHelper clientHelper, IConfiguration configuration)
    {
        _restClientHelper = clientHelper;
        _gatewayServiceUrl = configuration["GatewayServiceUrl"];

        _sendKudosPage = configuration["SendKudosPage"];
    }


    public Task<MessagingExtensionResponse> ViewResponse(ITurnContext<IInvokeActivity> turnContext, MessagingExtensionQuery query)
    {

        var sentTo = MessageExtensionHelper.GetQueryData(query.Parameters, "sentTo");

        var sentFrom = MessageExtensionHelper.GetQueryData(query.Parameters, "sentFrom");

        Debug.WriteLine($"🔍 sentTo: {sentTo}");
        Debug.WriteLine($"🔍 sentFrom: {sentFrom}");

        // create the preview card
        // shown in the search results in Teams UI
        // shown in the references section of Copilot messages
        var previewCard = new ThumbnailCard { Title = "Super Kudos" };

        // var message = query?.Parameters?[2]?.Value as string;

        var templateJson = System.IO.File.ReadAllText(_adaptiveCardFilePath);

        var template = new AdaptiveCards.Templating.AdaptiveCardTemplate(templateJson);

        var adaptiveCardJson = template.Expand(new
        {
            to = sentTo,
            from = sentFrom
        });

        var adaptiveCard = AdaptiveCard.FromJson(adaptiveCardJson).Card;

        var attachment = new MessagingExtensionAttachment
        {
            ContentType = AdaptiveCard.ContentType,
            Content = adaptiveCard,
            Preview = previewCard.ToAttachment()
        };

        // create an an array of attachments to be sent in the response
        var attachments = new List<MessagingExtensionAttachment>();
        attachments.Add(attachment);

        var response = new MessagingExtensionResponse
        {
            ComposeExtension = new MessagingExtensionResult
            {
                Type = "result",
                AttachmentLayout = "list",
                Attachments = attachments
            }
        };

        return Task.FromResult(response as MessagingExtensionResponse);

    }
}
