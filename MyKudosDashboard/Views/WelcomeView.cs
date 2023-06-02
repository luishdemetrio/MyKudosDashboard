using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Logging;
using MyKudosDashboard.Interfaces;
using MyKudosDashboard.MessageSender;
using MyKudosDashboard.Models;
using Newtonsoft.Json;

namespace MyKudosDashboard.Views;

public class WelcomeView : IWelcomeView
{

    

    private IUserGateway _userGateway;

    
    private static string _kudosSentDashboard = string.Empty;

    private static string _likeSentDashboard = string.Empty;
    private static string _likeUndoDashboard = string.Empty;

    private static string _commentSentDashboard = string.Empty;
    private static string _commentDeletedDashboard = string.Empty;

   

    public WelcomeView(IUserGateway userGateway)
    {

        _userGateway = userGateway;

      
    }



    

    public async Task<string> GetUserPhoto(string userId)
    {

        return await _userGateway.GetUserPhoto(userId);
    }

    

}
