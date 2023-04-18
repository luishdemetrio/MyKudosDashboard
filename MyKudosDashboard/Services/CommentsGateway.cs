using MyKudos.Communication.Helper.Interfaces;
using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;
using RestSharp;

namespace MyKudosDashboard.Services;

public class CommentsGateway : ICommentsGateway
{

    private readonly string _gatewayServiceUrl;

    private IRestClientHelper _restClientHelper;

    private readonly ILogger<CommentsGateway> _logger;

    public CommentsGateway(IConfiguration config, ILogger<CommentsGateway> log, IRestClientHelper clientHelper)
    {
        _gatewayServiceUrl = config["GatewayServiceUrl"];
        _logger = log;
        _restClientHelper = clientHelper;
    }

    public async Task<bool> LikeComment(LikeComment like)
    {

        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<LikeComment, bool>($"{_gatewayServiceUrl}likescomment", Method.Post, like);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing LikeComment: {ex.Message}");
        }

        return result;

    }

    public async Task<bool> UndoLikeComment(LikeComment like)
    {

        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<LikeComment, bool>($"{_gatewayServiceUrl}likescomment", Method.Delete, like);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing UndoLikeComment: {ex.Message}");
        }

        return result;

    }

    public async Task<string> SendCommentsAsync(CommentsRequest comment)
    {
        string result = string.Empty;

        try
        {
            result = await _restClientHelper.SendApiData<CommentsRequest, string>($"{_gatewayServiceUrl}Comments", Method.Post, comment);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing SendCommentsAsync: {ex.Message}");
        }

        return result;

    }

    public async Task<IEnumerable<CommentsResponse>> GetComments(string kudosId)
    {
        List<CommentsResponse> result = new();

        try
        {

            var comments  = await _restClientHelper.GetApiData<IEnumerable<CommentsResponse>>($"{_gatewayServiceUrl}Comments?kudosId={kudosId}");
            result = comments.ToList();
        
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetComments: {ex.Message}");
        }

        return result;

    }

    public async Task<bool> UpdateComments(CommentsRequest comments)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<CommentsRequest, bool>($"{_gatewayServiceUrl}Comments", Method.Put, comments);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing UpdateComments: {ex.Message}");
        }

        return result;
    }

    public async Task<bool> DeleteComments(CommentsRequest comments)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<CommentsRequest, bool>($"{_gatewayServiceUrl}Comments", Method.Delete, comments);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing DeleteComments: {ex.Message}");
        }

        return result;

    }
}
