using Azure.Messaging.ServiceBus;

namespace MyKudosDashboard.MessageSender;

public class ServiceBusProcessorConfig
{
    public string DashboardName { get; set; }
    public string SubscriptionName { get; set; }
    public Func<ProcessMessageEventArgs, Task> MessageProcessor { get; set; }
}
