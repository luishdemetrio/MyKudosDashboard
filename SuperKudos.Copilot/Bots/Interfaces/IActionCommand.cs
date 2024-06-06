using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace SuperKudos.Copilot.Bots;

public interface IActionCommand
{

    public Task<AdaptiveCardInvokeResponse> Execute(ITurnContext<IInvokeActivity> turnContext,
                                                    AdaptiveCardInvokeValue invokeValue, CancellationToken cancellationToken);
}
