using MyKudos.Kudos.Token.Interfaces;
using RestSharp;

namespace MyKudos.Test.Console;

public  class GatewayService
{

    private string _gatewayServiceUrl = "https://superkudosgateway.azurewebsites.net/";

    private readonly IRestServiceToken _serviceToken;

    public GatewayService(IRestServiceToken restServiceToken)
    {
        _serviceToken = restServiceToken;

    }


    public async Task<bool> GetRecognitionsAsync()
    {

        var uri = $"{_gatewayServiceUrl}recognition";

        var client = new RestClient(uri);

        var token = await _serviceToken.GetAccessTokenAsync();

        var request = new RestRequest();
        request.Method = Method.Get;
        request.AddHeader("Authorization", "Bearer " + token);


        RestResponse response = client.Execute(request);

        return (response.IsSuccessful);
        

    }

}
