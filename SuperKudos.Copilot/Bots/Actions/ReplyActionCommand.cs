using AdaptiveCards;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gateway.Domain.Models;
using Newtonsoft.Json.Linq;

namespace SuperKudos.Copilot.Bots;

public class ReplyActionCommand : IActionCommand
{
    private IRestClientHelper _restClientHelper;
    private string _gatewayServiceUrl;

    private readonly string _adaptiveCardFilePath = Path.Combine(".", "Resources", "KudosLikedCard.json");

    public ReplyActionCommand(IRestClientHelper clientHelper, IConfiguration configuration)
    {
        _restClientHelper = clientHelper;

        _gatewayServiceUrl = configuration["GatewayServiceUrl"];

    }

    public async Task<AdaptiveCardInvokeResponse> Execute(ITurnContext<IInvokeActivity> turnContext,
                                                          AdaptiveCardInvokeValue invokeValue, 
                                                          CancellationToken cancellationToken)
    {
        // Get the data object from the invokeValue
        JObject data = (JObject)invokeValue.Action.Data;

        var kudosId = data.Value<int>("kudosId");
        var userProfileId = new Guid(data.Value<string>("userProfileId"));
        var reply = data.Value<string>("replyText");


        var comments = new CommentsRequest { KudosId = kudosId,
                                         FromPersonId = userProfileId, 
                                         Date = DateTime.Now, 
                                         Message = reply };


        bool likeSent = await _restClientHelper.SendApiData<CommentsRequest, bool>(
                                        $"{_gatewayServiceUrl}comments", HttpMethod.Post, comments);

        var templateJson = System.IO.File.ReadAllText(_adaptiveCardFilePath);

        var template = new AdaptiveCards.Templating.AdaptiveCardTemplate(templateJson);

        var previewCard = new ThumbnailCard { Title = "Super Kudos" };

        var recognition = data.Value<string>("recognition");
        var sentTo = data.Value<string>("sentTo");
        var sentFrom = data.Value<string>("sentFrom");
        var fromPersonImage = data.Value<string>("fromPersonImage");
        var message = data.Value<string>("message");
        var sentOn = data.Value<DateTime>("sentOn");

        var adaptiveCardJson = template.Expand(new
        {
            name = sentTo,
            recognition = recognition,
            fromPersonImage = fromPersonImage,
            message = message,
            from = sentFrom,
            senton = sentOn,
            kudosId = kudosId,
            userProfileId = userProfileId,
            status = "Reply sent!"
        });

        var adaptiveCard = AdaptiveCard.FromJson(adaptiveCardJson).Card;

        //var attachment = new MessagingExtensionAttachment
        //{
        //    ContentType = AdaptiveCard.ContentType,
        //    Content = adaptiveCard,
        //    Preview = previewCard.ToAttachment()
        //};


        var response = new AdaptiveCardInvokeResponse()
        {
            StatusCode = StatusCodes.Status200OK,
            Type = AdaptiveCard.ContentType,
            Value = adaptiveCard
        };

        return response;
    }
}
