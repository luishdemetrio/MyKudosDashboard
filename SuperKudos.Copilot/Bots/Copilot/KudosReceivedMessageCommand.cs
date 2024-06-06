using AdaptiveCards;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gateway.Domain.Models;
using SuperKudos.Copilot.Helpers;
using System.Diagnostics;

namespace SuperKudos.Copilot.Bots;

public class KudosReceivedMessageCommand : IMessageCommand
{
    private readonly string _adaptiveCardFilePath = Path.Combine(".", "Resources", "KudosCard.json");

    private IRestClientHelper _restClientHelper;
    private string _gatewayServiceUrl;

    public KudosReceivedMessageCommand(IRestClientHelper clientHelper, IConfiguration configuration)
    {
        _restClientHelper = clientHelper;
        _gatewayServiceUrl = configuration["GatewayServiceUrl"];
    }

    public async Task<MessagingExtensionResponse> ViewResponse(ITurnContext<IInvokeActivity> turnContext,
                                                               MessagingExtensionQuery query)
    {

        Debug.WriteLine($"🔍 name: {query?.Parameters?[1]?.Value}");
        Debug.WriteLine($"🔍 number of days: {query?.Parameters?[0]?.Value}");

        var templateJson = System.IO.File.ReadAllText(_adaptiveCardFilePath);

        var template = new AdaptiveCards.Templating.AdaptiveCardTemplate(templateJson);

        int fromNumberOfDays = 0;

        int.TryParse(query?.Parameters?[0]?.Value as string, out fromNumberOfDays);

        var name= query?.Parameters?[1]?.Value as string ?? string.Empty;

        var kudos = await FindKudos(name, fromNumberOfDays);


        // We take every row of the results and wrap them in cards wrapped in in MessagingExtensionAttachment objects.
        var attachments = kudos.Select(package =>
        {
            var previewCard = new ThumbnailCard { Title = "Super Kudos" };

            //as kudos can be sent to multiple people
            //we need to select the right person
            var person = package.Receivers.Find(package => package.Name.Contains(name));

            var adaptiveCardJson = template.Expand( new
                                                    {
                                                        name = person.Name,
                                                        message = package.Message,
                                                        from = package.From.Name,
                                                        senton = package.SendOn.ToShortDateString(),
                                                        kudosId = package.Id,
                                                        userProfileId = person.Id,
                                                        fromPersonImage = package.From.Photo,
                                                        recognition = package.Title,
                                                        likeButtonTittle = "Like",
                                                        status = string.Empty
            });

            var adaptiveCard = AdaptiveCard.FromJson(adaptiveCardJson).Card;
          
            var attachment = new MessagingExtensionAttachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = adaptiveCard,
                Preview = previewCard.ToAttachment()
            };

            return attachment;
        }).ToList();

        return new MessagingExtensionResponse
        {
            ComposeExtension = new MessagingExtensionResult
            {
                Type = "result",
                AttachmentLayout = "list",
                Attachments = attachments
            }
        };

    }

    private async Task<IEnumerable<KudosResponse>> FindKudos(string name, int fromNumberOfDays)
    {
        
        return await _restClientHelper.GetApiData<IEnumerable<KudosResponse>>($"{_gatewayServiceUrl}Kudos/GetKudosByName?name={name}&pageSize=10&fromNumberOfDays={fromNumberOfDays}&useSmallPhoto=true");
      
    }
}
