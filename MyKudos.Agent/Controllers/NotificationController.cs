using MyKudos.Agent.Models;
using AdaptiveCards.Templating;
using Microsoft.AspNetCore.Mvc;
using Microsoft.TeamsFx.Conversation;
using Newtonsoft.Json;
using MyKudos.Agent.Interfaces;

namespace MyKudos.Agent.Controllers
{
    [Route("api/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly ConversationBot _conversation;
        private IAgentNotification _agentNotification;

        //private readonly string _adaptiveCardFilePath = Path.Combine(".", "Resources", "NotificationDefault.json");

        public NotificationController(ConversationBot conversation, IAgentNotification agentNotification)
        {
            _conversation = conversation;
            _agentNotification = agentNotification;
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(CancellationToken cancellationToken = default)
        {

            await _agentNotification.SendNotification(new Kudos(Id: "1", From: "", To: "", Title: "", Message: "", SendOn: DateTime.Now ));

            //// Read adaptive card template
            //var cardTemplate = await System.IO.File.ReadAllTextAsync(_adaptiveCardFilePath, cancellationToken);

            //var installations = await this._conversation.Notification.GetInstallationsAsync(cancellationToken);
            //foreach (var installation in installations)
            //{
            //    // Build and send adaptive card
            //    var cardContent = new AdaptiveCardTemplate(cardTemplate).Expand
            //    (
            //        new NotificationDefaultModel
            //        {
            //            Title = "New Event Occurred!",
            //            AppName = "Contoso App Notification",
            //            Description = $"This is a sample http-triggered notification to {installation.Type}",
            //            NotificationUrl = "https://www.adaptivecards.io/",
            //        }
            //    );
            //    await installation.SendAdaptiveCard(JsonConvert.DeserializeObject(cardContent), cancellationToken);
            //}

            return Ok();
        }
    }
}
