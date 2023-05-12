using MyKudos.CardNotification.Models;
using AdaptiveCards.Templating;
using Microsoft.AspNetCore.Mvc;
using Microsoft.TeamsFx.Conversation;
using Newtonsoft.Json;

namespace MyKudos.CardNotification.Controllers
{
    [Route("api/cardnotification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly ConversationBot _conversation;
        private readonly string _adaptiveCardFilePath = Path.Combine(".", "Resources", "NotificationDefault.json");
        private IConfiguration _configuration;

      //  private ILogger _logger;

        public NotificationController(ConversationBot conversation, IConfiguration configuration)
        {

            this._conversation = conversation;
            this._configuration = configuration;
          //  _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(CancellationToken cancellationToken = default)
        {
            int membersCount = 0;
            string users = string.Empty;

            var installations = await this._conversation.Notification.GetInstallationsAsync(cancellationToken);

            if (installations.Count() == 0)
            {
            //    _logger.LogWarning($"There are no users with the bot {_configuration.GetSection("BOT_ID")?.Value} installed");

                return Ok($"There are no users with the bot: {_configuration.GetSection("BOT_ID")?.Value} installed");
            }

            using var content = new StreamContent(this.HttpContext.Request.Body);

            var contentString = await content.ReadAsStringAsync();

            Kudos kudos = null;

            if (!string.IsNullOrEmpty(contentString))
            {
                kudos = System.Text.Json.JsonSerializer.Deserialize<Kudos>(contentString);

                if (kudos == null)
                {
                //    _logger.LogWarning($"The Kudos body is missing or is invalid");
                    return Ok("The Kudos body is missing or is invalid");
                }
            }

            // Read adaptive card template
            var cardTemplate = await System.IO.File.ReadAllTextAsync(_adaptiveCardFilePath, cancellationToken);

            foreach (var installation in installations)
            {


                // "Person" means this bot is installed as Personal app
                if (installation.Type == NotificationTargetType.Person)
                {
                    var members = await installation.GetMembersAsync(cancellationToken);

                    if (members == null)
                    {
                     //   _logger.LogWarning("The bot has no members.");
                        return Ok("The bot has no members..");
                    }

                    membersCount += members.Count();


                    // find the people (who received the reward, who sent it and his/her boss)
                    var sendTo = members.ToList().FindAll(m =>
                                (m.Account.AadObjectId == kudos.To.Id) ||
                                (m.Account.AadObjectId == kudos.From.Id) ||
                                (m.Account.AadObjectId == kudos.ManagerId));


                    for (int i = 0; i < members.Length; i++)
                    {
                        users = users + members[i].Account.UserPrincipalName + " - " + members[i].Account.Name + "; BotAppId:" + installation.BotAppId;
                    }

                    foreach (var to in sendTo)
                    {

                        // Build and send adaptive card
                        var cardContent = new AdaptiveCardTemplate(cardTemplate).Expand(kudos);

                        await to.SendAdaptiveCard(JsonConvert.DeserializeObject(cardContent), cancellationToken);

                    }

                }
            }

            //_logger.LogInformation($"Installations: {installations.Count()}\nMembers: {membersCount}\nUsers:{users}");
            return Ok($"Installations: {installations.Count()}\nMembers: {membersCount}\nUsers:{users}");
        }
    }
}
