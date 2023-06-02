using MyKudosDashboard.EventHub.Enums;

namespace MyKudosDashboard.EventHub;

public class EventHubResponse<TOptions, TEvent>
{
    public TOptions Kind { get; set; }
    public TEvent Event { get; set; }
}
