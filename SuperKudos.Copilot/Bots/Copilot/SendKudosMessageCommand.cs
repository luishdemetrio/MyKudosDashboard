using AdaptiveCards;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Graph.Models;
using MyKudos.Communication.Helper.Interfaces;
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

        //var previewCard = new ThumbnailCard { Title = "Super Kudos" };
        var previewCard = new ThumbnailCard ();

        var sentTo = query?.Parameters?[0]?.Value as string;

        var sentFrom = query?.Parameters?[1]?.Value as string ;

       // var message = query?.Parameters?[2]?.Value as string;

        var templateJson = System.IO.File.ReadAllText(_adaptiveCardFilePath);

        var template = new AdaptiveCards.Templating.AdaptiveCardTemplate(templateJson);


        var adaptiveCardJson = template.Expand(new
        {
            name = sentTo,
            from = sentFrom//,
            //copilot_generateText= message

        });

        var adaptiveCard = AdaptiveCard.FromJson(adaptiveCardJson).Card;



        var attachment = new MessagingExtensionAttachment
        {
            ContentType = AdaptiveCard.ContentType,
            Content = adaptiveCard,
            Preview = previewCard.ToAttachment()
        };

        var response = new MessagingExtensionResponse
        {
            ComposeExtension = new MessagingExtensionResult
            {
                Type = "result",
                AttachmentLayout = "list",
                Attachments = new List<MessagingExtensionAttachment> { attachment }
            }
        };

        return Task.FromResult(response);

    }
}
