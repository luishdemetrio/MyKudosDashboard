using Microsoft.Bot.Schema.Teams;

namespace SuperKudos.Copilot.Helpers;

public class MessageExtensionHelper
{
    public static string GetQueryData(IList<MessagingExtensionParameter> parameters, string key)
    {
        // if no parameters were passed in then return an empty string
        if (parameters.Any() != true)
        {
            return string.Empty;
        }

        // find the parameter with the specified key and return the value
        var foundPair = parameters.FirstOrDefault(pair => pair.Name == key);
        return foundPair?.Value?.ToString() ?? string.Empty;
    }
}
