using AdaptiveCards.Templating;
using Microsoft.TeamsFx.Conversation;
using MyKudos.Agent.Interfaces;
using MyKudos.Agent.Models;
using Newtonsoft.Json;
using System.Threading;

namespace MyKudos.Agent.Services;

public class AgentNotification : IAgentNotification
{
    private readonly ConversationBot _conversation;
    private readonly string _adaptiveCardFilePath = Path.Combine(".", "Resources", "NotificationDefault.json");

    public AgentNotification(ConversationBot conversation)
    {
        this._conversation = conversation;
    }

    public async Task<bool> SendNotification(Kudos kudos)
    {
        CancellationToken cancellationToken = default;

        // Read adaptive card template
        var cardTemplate = await System.IO.File.ReadAllTextAsync(_adaptiveCardFilePath, cancellationToken);

        var installations = await this._conversation.Notification.GetInstallationsAsync(cancellationToken);
        foreach (var installation in installations)
        {
            // Build and send adaptive card
            var cardContent = new AdaptiveCardTemplate(cardTemplate).Expand
            (
                new NotificationDefaultModel
                {
                    Title = "New Event Occurred!",
                    AppName = "Contoso App Notification",
                    Description = $"This is a sample http-triggered notification to {installation.Type}",
                    NotificationUrl = "https://www.adaptivecards.io/",
                }
            );
            await installation.SendAdaptiveCard(JsonConvert.DeserializeObject(cardContent), cancellationToken);

            
        }

        return true;
    }
}
