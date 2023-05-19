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

    private ServiceBusClient _serviceBusClient;
    private ServiceBusProcessor _serviceBusScoreProcessor;

    private string _userId;


    private IUserGateway _userGateway;

    
    private static string _kudosSentDashboard = string.Empty;

    private static string _likeSentDashboard = string.Empty;
    private static string _likeUndoDashboard = string.Empty;

    private static string _commentSentDashboard = string.Empty;
    private static string _commentDeletedDashboard = string.Empty;

    //public IWelcomeView.UpdateLikesCallBack LikeCallback { get; set; }

    //public IWelcomeView.UpdateLikesCallBack UndoLikeCallback { get; set; }

    //public IWelcomeView.UpdateKudosCallBack KudosCallback { get; set; }

    //public IWelcomeView.CommentsCallBack CommentsSentCallback { get; set; }
    //public IWelcomeView.CommentsCallBack CommentsUpdatedCallback { get; set; }
    //public IWelcomeView.CommentsCallBack CommentsDeletedCallback { get; set; }

    //private ServiceBusSubscriberHelper _subscriberLikeSent;
    //private ServiceBusSubscriberHelper _subscriberUndoLike;
    //private ServiceBusSubscriberHelper _subscriberKudosSent;
    //private ServiceBusSubscriberHelper _subscriberCommentsSent;
    //private ServiceBusSubscriberHelper _subscriberCommentsDeleted;

    public WelcomeView(IUserGateway userGateway)
    {

        _userGateway = userGateway;

        //_likeSentDashboard = configuration["KudosServiceBus_LikeSentDashboard"];
        //_likeUndoDashboard = configuration["KudosServiceBus_LikeUndoDashboard"];

        //_kudosSentDashboard = configuration["KudosServiceBus_KudosSentDashboard"];

        //_commentSentDashboard = configuration["KudosServiceBus_MessageSentDashboard"];
        //_commentDeletedDashboard = configuration["KudosServiceBus_MessageDeletedDashboard"];

        //_subscriberLikeSent = new ServiceBusSubscriberHelper(configuration, logger);
        //_subscriberUndoLike = new ServiceBusSubscriberHelper(configuration, logger);
        //_subscriberKudosSent = new ServiceBusSubscriberHelper(configuration, logger);
        //_subscriberCommentsDeleted = new ServiceBusSubscriberHelper(configuration, logger);
        //_subscriberCommentsSent = new ServiceBusSubscriberHelper(configuration, logger);
    }



    

    public async Task<string> GetUserPhoto(string userId)
    {

        return await _userGateway.GetUserPhoto(userId);
    }

    

    
    //private void SubscribeToLikeSent(string subscriptionName)
    //{
    //    var config = new ServiceBusProcessorConfig
    //    {
    //        DashboardName = _likeSentDashboard,
    //        SubscriptionName = subscriptionName,
    //        MessageProcessor = async arg =>
    //        {
    //            //retrive the message body
    //            var like = JsonConvert.DeserializeObject<Like>(arg.Message.Body.ToString());

    //            if (like != null)
    //            {
    //                LikeCallback?.Invoke(like).ConfigureAwait(true);
    //            }

    //            await arg.CompleteMessageAsync(arg.Message);
    //        }
    //    };

    //    _subscriberLikeSent.ServiceBusProcessor(config);
    //}

    //private void SubscribeUndoToLikeSent(string subscriptionName)
    //{
    //    var config = new ServiceBusProcessorConfig
    //    {
    //        DashboardName = _likeUndoDashboard,
    //        SubscriptionName = subscriptionName,
    //        MessageProcessor = async arg =>
    //        {
    //            //retrive the message body
    //            var like = JsonConvert.DeserializeObject<Like>(arg.Message.Body.ToString());

    //            if (like != null)
    //            {
    //                UndoLikeCallback?.Invoke(like).ConfigureAwait(true);
    //            }

    //            await arg.CompleteMessageAsync(arg.Message);
    //        }
    //    };

    //    _subscriberUndoLike.ServiceBusProcessor(config);
    //}


    //private void SubscribeKudosSent(string subscriptionName)
    //{
    //    var config = new ServiceBusProcessorConfig
    //    {
    //        DashboardName = _kudosSentDashboard,
    //        SubscriptionName = subscriptionName,
    //        MessageProcessor = async arg =>
    //        {
    //            //retrive the message body
    //            var kudos = JsonConvert.DeserializeObject<KudosResponse>(arg.Message.Body.ToString());

    //            if (kudos != null)
    //            {
    //                await KudosCallback?.Invoke(kudos);
    //            }

    //            await arg.CompleteMessageAsync(arg.Message);
    //        }
    //    };

    //    _subscriberKudosSent.ServiceBusProcessor(config);
    //}

    //private void SubscribeCommentSent(string subscriptionName)
    //{
    //    var config = new ServiceBusProcessorConfig
    //    {
    //        DashboardName = _commentSentDashboard,
    //        SubscriptionName = subscriptionName,
    //        MessageProcessor = async arg =>
    //        {

    //            //retrive the message body
    //            var comments = JsonConvert.DeserializeObject<CommentsRequest>(arg.Message.Body.ToString());

    //            if (comments != null)
    //            {
    //                await CommentsSentCallback?.Invoke(comments);
    //            }

    //            await arg.CompleteMessageAsync(arg.Message);
    //        }
    //    };

    //    _subscriberCommentsSent.ServiceBusProcessor(config);
    //}

    //private void SubscribeCommentDeleted(string subscriptionName)
    //{
    //    var config = new ServiceBusProcessorConfig
    //    {
    //        DashboardName = _commentDeletedDashboard,
    //        SubscriptionName = subscriptionName,
    //        MessageProcessor = async arg =>
    //        {

    //            //retrive the message body
    //            var comments = JsonConvert.DeserializeObject<CommentsRequest>(arg.Message.Body.ToString());

    //            if (comments != null)
    //            {
    //                await CommentsDeletedCallback?.Invoke(comments);
    //            }

    //            await arg.CompleteMessageAsync(arg.Message);
    //        }
    //    };

    //    _subscriberCommentsDeleted.ServiceBusProcessor(config);
    //}

}
