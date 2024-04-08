using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;

namespace SuperKudos.Copilot.Helpers;

public static class AuthenticationHelpers
{
    public static async Task<TokenResponse> GetToken(UserTokenClient userTokenClient, string state, string userId, 
                                                     string channelId, string connectionName, CancellationToken cancellationToken)
    {
        var magicCode = string.Empty;

        if (!string.IsNullOrEmpty(state))
        {
            if (int.TryParse(state, out var parsed))
            {
                magicCode = parsed.ToString();
            }
        }

        return await userTokenClient.GetUserTokenAsync(userId, connectionName, channelId, magicCode, cancellationToken);
    }

    public static bool HasToken(TokenResponse tokenResponse)
    {
        return tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.Token);
    }

    public static async Task<MessagingExtensionResponse> CreateAuthResponse(UserTokenClient userTokenClient, string connectionName, Activity activity, CancellationToken cancellationToken)
    {
        // get the sign in resource
        var resource = await userTokenClient.GetSignInResourceAsync(connectionName, activity, null, cancellationToken);

        return new MessagingExtensionResponse
        {
            ComposeExtension = new MessagingExtensionResult
            {
                Type = "auth",
                SuggestedActions = new MessagingExtensionSuggestedAction
                {
                    Actions =
                    [
                        new() {
                            Type = ActionTypes.OpenUrl,
                            Value = resource.SignInLink,
                            Title = "Sign In",
                        },
                    ]
                }
            }
        };
    }

    public static async Task<AdaptiveCardInvokeResponse> CreateOAuthCardResponse(UserTokenClient userTokenClient, string connectionName, Activity activity, CancellationToken cancellationToken)
    {
        // get the sign in resource
        var resource = await userTokenClient.GetSignInResourceAsync(connectionName, activity, null, cancellationToken);

        return new AdaptiveCardInvokeResponse
        {
            StatusCode = 401,
            Type = $"{Activity.ContentType}.loginRequest",
            Value = JObject.FromObject(
                new OAuthCard
                {
                    Buttons = [
                        new() {
                            Title = "Sign In",
                            Type = ActionTypes.Signin,
                            Value = resource.SignInLink
                        }
                    ],
                    Text = "Please sign in to continue.",
                    ConnectionName = connectionName
                }
            )
        };
    }

    public static async Task SignOut(UserTokenClient userTokenClient, string userId, string channelId, string connectionName, CancellationToken cancellationToken)
    {
        await userTokenClient.SignOutUserAsync(userId, connectionName, channelId, cancellationToken);
    }
}
