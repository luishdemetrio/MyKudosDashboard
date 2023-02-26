using Microsoft.Identity.Client;
using MyKudos.Kudos.Token.Interfaces;

namespace MyKudos.Kudos.Token.Services;

public class RestServiceToken : IRestServiceToken
{

    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _tenantId;
    private readonly string _exposedAPI;
    private readonly IConfidentialClientApplication _confidentialClientApplication;

    public RestServiceToken(string clientId, string clientSecret, string tenantId, string exposedAPI)
    {

        _clientId = clientId;
        _clientSecret = clientSecret;
        _tenantId = tenantId;
        _exposedAPI = exposedAPI;

        _confidentialClientApplication = ConfidentialClientApplicationBuilder.Create(_clientId)
        .WithClientSecret(_clientSecret)
        .WithAuthority(new Uri($"https://login.microsoftonline.com/{_tenantId}"))
        .Build();


    }

    public async Task<string> GetAccessTokenAsync()
    {

        var scopes = new string[] { _exposedAPI };

        var result = await _confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();

        return result.AccessToken;
    }
}