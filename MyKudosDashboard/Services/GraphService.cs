using Azure.Core;
using Azure.Identity;
using Microsoft.Graph;
using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;
using Newtonsoft.Json;
using RestSharp;
using System.IO;

namespace MyKudosDashboard.Services;

public class GraphService : IGraphService
{

    // App-ony auth token credential
    private ClientSecretCredential _clientSecretCredential;

    // Client configured with app-only authentication
    private static GraphServiceClient _appClient;

    //

    public GraphService()
    {
        var settings = Settings.LoadSettings(); ;

        if (_clientSecretCredential == null)
        {
            _clientSecretCredential = new ClientSecretCredential(
                settings.TenantId, settings.ClientId, settings.ClientSecret);
        }

        if (_appClient == null)
        {
            _appClient = new GraphServiceClient(_clientSecretCredential,
                // Use the default scope, which will request the scopes
                // configured on the app registration
                new[] { "https://graph.microsoft.com/.default" });
        }
    }

    public async Task<string> GetAppOnlyTokenAsync()
    {
        // Ensure credential isn't null
        _ = _clientSecretCredential ??
            throw new NullReferenceException("Graph has not been initialized for app-only auth");

        // Request token with given scopes
        var context = new TokenRequestContext(new[] { "https://graph.microsoft.com/.default" });
        var response = await _clientSecretCredential.GetTokenAsync(context);
        return response.Token;
    }


    public async Task<string> GetUserPhoto(string userid)
    {


        System.IO.Stream photo =  await _appClient.Users[userid].Photos["48x48"].Content
            .Request()
            .GetAsync();
        
        using MemoryStream ms = new MemoryStream();
        photo.CopyTo(ms);

        return "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());

    }


}
