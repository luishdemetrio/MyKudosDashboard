using Azure.Messaging.EventGrid;
using Azure.Messaging.EventGrid.SystemEvents;
using Microsoft.AspNetCore.Mvc;
using MyKudosDashboard.EventGrid;

namespace MyKudosDashboard.Controller;

[Route("api/[controller]")]
[ApiController]
public class EventGridReceiverController : ControllerBase
{

    private IEventGridKudosReceived _eventGridKudosReceived;
    private IEventGridUserPointsReceived _eventGridUsePointsReceived;
    

    public EventGridReceiverController(IEventGridKudosReceived eventGridReceived, IConfiguration configuration, 
                                       IEventGridUserPointsReceived eventGridUsePointsReceived)
    {
        _eventGridKudosReceived = eventGridReceived;
        _eventGridUsePointsReceived = eventGridUsePointsReceived;
     
    }

    [HttpPost(Name = "HandleEvents")]
    public IActionResult HandleEvents([FromBody] EventGridEvent[] events)
    {
        
        string response = string.Empty;

        foreach (EventGridEvent eventGridEvent in events)
        {
            // Handle system events
            if (eventGridEvent.TryGetSystemEventData(out object eventData))
            {
                // Handle the subscription validation event
                if (eventData is SubscriptionValidationEventData subscriptionValidationEventData)
                {
                    // Do any additional validation (as required) and then return back the below response
                    var responseData = new
                    {
                        ValidationResponse = subscriptionValidationEventData.ValidationCode
                    };

                    return new OkObjectResult(responseData);
                }
            } // Handle the custom event
            else 
            {
                switch (eventGridEvent.EventType)
                {

                    case "SendKudosDashboard":
                        _eventGridKudosReceived.NotifyKudosSentUpdate(eventGridEvent.Data.ToObjectFromJson<string>());
                        break;

                    case "LikeSentDashboard":
                        _eventGridKudosReceived.NotifyLikesSentUpdate(eventGridEvent.Data.ToObjectFromJson<string>());
                        break;

                    case "LikeUndoDashboard":
                        _eventGridKudosReceived.NotifyUndoLikesSentUpdate(eventGridEvent.Data.ToObjectFromJson<string>());
                        break;

                    case "UpdateUserPointDashboard":
                        _eventGridUsePointsReceived.NotifyUserPointsUpdate(eventGridEvent.Data.ToObjectFromJson<string>());
                        break;

                    case "MessageSent":
                        _eventGridKudosReceived.NotifyMessageSentUpdate(eventGridEvent.Data.ToObjectFromJson<string>());
                        break;

                    case "MessageDeleted":
                        _eventGridKudosReceived.NotifyMessageDeletedUpdate(eventGridEvent.Data.ToObjectFromJson<string>());
                        break;

                    case "MessageUpdated":
                        _eventGridKudosReceived.NotifyMessageUpdatedUpdate(eventGridEvent.Data.ToObjectFromJson<string>());
                        break;

                    default:
                        break;
                }
                

            }
        }
        return new OkObjectResult(response);

        
    }

}
