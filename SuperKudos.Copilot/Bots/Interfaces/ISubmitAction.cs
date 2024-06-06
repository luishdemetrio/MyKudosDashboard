using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Bot.Schema;

namespace SuperKudos.Copilot.Bots;

public interface ISubmitAction
{
    Task<MessagingExtensionActionResponse> SubmitAction(ITurnContext<IInvokeActivity> turnContext,
                                                                 MessagingExtensionAction action);
}