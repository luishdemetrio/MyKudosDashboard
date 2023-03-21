using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;

namespace MyKudos.Notification.Bot;


public sealed class KudosNotificationBot : TeamsActivityHandler
{

    protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
    {
        turnContext.Activity.RemoveRecipientMention();
        var text = turnContext.Activity.Text.Trim().ToLower();
              
    }

    protected override async Task OnTeamsMembersAddedAsync(IList<TeamsChannelAccount> membersAdded, TeamInfo teamInfo, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
    {
        foreach (var teamMember in membersAdded)
        {
            if (teamMember.Id != turnContext.Activity.Recipient.Id && turnContext.Activity.Conversation.ConversationType != "personal")
            {
                await turnContext.SendActivityAsync(MessageFactory.Text($"Welcome to the team {teamMember.GivenName} {teamMember.Surname}."), cancellationToken);
            }
        }
    }

    protected override async Task OnInstallationUpdateActivityAsync(ITurnContext<IInstallationUpdateActivity> turnContext, CancellationToken cancellationToken)
    {
        if (turnContext.Activity.Conversation.ConversationType == "channel")
        {
            await turnContext.SendActivityAsync($"Welcome to Microsoft Teams conversationUpdate events demo bot. This bot is configured in {turnContext.Activity.Conversation.Name}");
        }
        else
        {
            await turnContext.SendActivityAsync("Welcome to Microsoft Teams conversationUpdate events demo bot.");
        }
    }

}
