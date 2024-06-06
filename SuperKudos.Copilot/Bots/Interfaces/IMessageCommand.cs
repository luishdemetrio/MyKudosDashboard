using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Bot.Schema;

namespace SuperKudos.Copilot.Bots;

public interface IMessageCommand
{
    Task<MessagingExtensionResponse> ViewResponse(ITurnContext<IInvokeActivity> turnContext, MessagingExtensionQuery query);

}
