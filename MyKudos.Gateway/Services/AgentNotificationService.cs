using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gateway.Domain.Models;
using MyKudos.Gateway.Interfaces;
using Newtonsoft.Json;
using RestSharp;

using System;
using System.Drawing;
using System.IO;


namespace MyKudos.Gateway.Services;

public class AgentNotificationService : IAgentNotificationService
{

    private readonly string _agentServiceUrl;
    private IRestClientHelper _restClientHelper;

    private readonly ILogger<KudosServiceRest> _logger;

    public AgentNotificationService(IConfiguration config, IRestClientHelper clientHelper, ILogger<KudosServiceRest> logger)
    {
        _agentServiceUrl = config["agentServiceUrl"];
        _restClientHelper = clientHelper;
        _logger = logger;
    }

    private string ResizeBase64Image(string base64Image, int newWidth, int newHeight)
    {
        byte[] imageBytes = Convert.FromBase64String(base64Image);

        using MemoryStream inputStream = new MemoryStream(imageBytes);
        using Image originalImage = Image.FromStream(inputStream);
        using Image resizedImage = originalImage.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero);
        using MemoryStream outputStream = new MemoryStream();

        resizedImage.Save(outputStream, originalImage.RawFormat);
        byte[] resizedImageBytes = outputStream.ToArray();

        return Convert.ToBase64String(resizedImageBytes);
    }

    public async Task<bool> SendNotificationAsync(Gateway.Domain.Models.KudosNotification kudos)
    {

        bool result = false;

        try
        {


            kudos.From.Photo = $"data:image/png;base64,{ResizeBase64Image(kudos.From.Photo.Replace("data:image/png;base64,", ""), 26, 26)}";

            foreach (var item in kudos.Receivers)
            {
                item.Photo = $"data:image/png;base64,{ResizeBase64Image(item.Photo.Replace("data:image/png;base64,", ""), 26, 26)}";
            }


            var uri = $"{_agentServiceUrl}";

            var client = new RestClient(uri);

            var request = new RestRequest();
            request.Method = Method.Post;

            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");

            var body = JsonConvert.SerializeObject(kudos);

            request.AddParameter("application/json", body, ParameterType.RequestBody);


            RestResponse response = client.Execute(request);

            return (response != null && response.StatusCode == System.Net.HttpStatusCode.OK);

            // result = await _restClientHelper.SendApiData<Kudos.Domain.Models.KudosNotification, bool>($"{_agentServiceUrl}api/notification", HttpMethod.Post, kudos);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing SendKudos: {ex.Message}");
        }

        return result;
    }



}
