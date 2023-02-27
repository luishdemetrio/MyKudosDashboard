//using Microsoft.Identity.Client;
//using MyKudos.Gateway.Interfaces;

//namespace MyKudos.Gateway.Services.Rest;

//public class RestServiceToken : IRestServiceToken
//{

//    private readonly string _clientId;
//    private readonly string _clientSecret;
//    private readonly string _tenantId;
//    private readonly string _exposedAPI;
//    private readonly IConfidentialClientApplication _confidentialClientApplication;

//    public RestServiceToken(IConfiguration config)
//    {

//        _clientId = config["ClientId"];
//        _clientSecret = config["ClientSecret"];
//        _tenantId = config["TenantId"];
//        _exposedAPI = config["ExposedApi"];

//        _confidentialClientApplication = ConfidentialClientApplicationBuilder.Create(_clientId)
//        .WithClientSecret(_clientSecret)
//        .WithAuthority(new Uri($"https://login.microsoftonline.com/{_tenantId}"))
//        .Build();


//    }

//    public async Task<string> GetAccessTokenAsync()
//    {

//        var scopes = new string[] { _exposedAPI };

//        var result = await _confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();

//        return result.AccessToken;
//    }
//}
