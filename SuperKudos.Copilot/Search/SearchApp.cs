﻿using AdaptiveCards;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using MyKudos.Communication.Helper.Interfaces;
using SuperKudos.Copilot.Bots;

namespace SuperKudos.Copilot.Search;

public class SearchApp : TeamsActivityHandler
{

    private IRestClientHelper _restClientHelper;
    private IConfiguration _configuration;

    private IKudosFetchTask _sendKudosFetchTask;
    private IKudosSubmitAction _kudosSubmitAction;
    public SearchApp(IRestClientHelper clientHelper, IConfiguration configuration, 
                     IKudosFetchTask sendKudosFetchTask,
                     IKudosSubmitAction kudosSubmitAction)
    {
        _restClientHelper = clientHelper;
        _configuration = configuration;

        _sendKudosFetchTask = sendKudosFetchTask;
        _kudosSubmitAction = kudosSubmitAction;

    }

    // Search via Copilot
    protected override async Task<MessagingExtensionResponse> OnTeamsMessagingExtensionQueryAsync(
                                                    ITurnContext<IInvokeActivity> turnContext,
                                                    MessagingExtensionQuery query,
                                                    CancellationToken cancellationToken)
    {


        IMessageCommand messageCommand = null;
        MessagingExtensionResponse response = null;

        switch (query.CommandId)
        {
            case "findKudosReceived":
            case "findKudosSentToMe":
            case "findRecognitionsReceived":
            case "findRecognitionsSentToMeBr":
                messageCommand = new KudosCopilotChatMessageCommand(_restClientHelper, _configuration);
                break;
        }

        if (messageCommand != null)
        {
            response = await messageCommand.ViewResponse(turnContext, query);
        }

        return response;

    }

    //Action handler for the adaptive card of Copilot
    protected override async Task<AdaptiveCardInvokeResponse> OnAdaptiveCardInvokeAsync(
                                                                ITurnContext<IInvokeActivity> turnContext,
                                                                AdaptiveCardInvokeValue invokeValue, 
                                                                CancellationToken cancellationToken)
    {
        IActionCommand actionCommand = null;
        AdaptiveCardInvokeResponse result = null;

        switch (invokeValue.Action.Verb)
        {
            case "send-like":
                actionCommand = new KudosCopilotChatLikeActionCommand(_restClientHelper, _configuration);
                break;

            case "send-reply":
                actionCommand = new KudosCopilotChatReplyActionCommand(_restClientHelper, _configuration);
                break;
        }

        if (actionCommand != null)
            result = await actionCommand.Execute(turnContext, invokeValue, cancellationToken);


        return result;
    }    

    protected override async Task<MessagingExtensionActionResponse> OnTeamsMessagingExtensionFetchTaskAsync(
                                                                        ITurnContext<IInvokeActivity> turnContext, 
                                                                        MessagingExtensionAction action,
                                                                        CancellationToken cancellationToken)
    {
        
        //return await _sendKudosFetchTask.SendKudosCreateFormCard(turnContext);

         return await _sendKudosFetchTask.SendKudosEmbeddedWebView(turnContext);

    }

    
    protected override async Task<MessagingExtensionActionResponse> OnTeamsMessagingExtensionSubmitActionAsync(
                                                                        ITurnContext<IInvokeActivity> turnContext, 
                                                                        MessagingExtensionAction action, 
                                                                        CancellationToken cancellationToken)
    {
        return await _kudosSubmitAction.SendKudosSubmitAction(turnContext, action);

    }

}



