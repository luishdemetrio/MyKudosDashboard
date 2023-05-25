using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Gateway.Domain.Models;
using MyKudosDashboard.Interfaces;
using MyKudosDashboard.Models;

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

    public async Task<bool> LikeComment(LikeCommentGateway like)
    {

        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<LikeCommentGateway, bool>($"{_gatewayServiceUrl}likescomment", HttpMethod.Post, like);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing LikeComment: {ex.Message}");
        }

        return result;

    }

    public async Task<bool> UndoLikeComment(LikeCommentGateway like)
    {

        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<LikeCommentGateway, bool>($"{_gatewayServiceUrl}likescomment", HttpMethod.Delete, like);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing UndoLikeComment: {ex.Message}");
        }

        return result;

    }

    public async Task<int> SendCommentsAsync(CommentsRequest comment)
    {
        int result = 0;

        try
        {
            result = await _restClientHelper.SendApiData<CommentsRequest, int>($"{_gatewayServiceUrl}Comments", HttpMethod.Post, comment);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing SendCommentsAsync: {ex.Message}");
        }

        return result;

    }

    public async Task<IEnumerable<CommentsResponse>> GetComments(int kudosId)
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
            result = await _restClientHelper.SendApiData<CommentsRequest, bool>($"{_gatewayServiceUrl}Comments", HttpMethod.Put, comments);
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
            result = await _restClientHelper.SendApiData<CommentsRequest, bool>($"{_gatewayServiceUrl}Comments", HttpMethod.Delete, comments);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing DeleteComments: {ex.Message}");
        }

        return result;

    }
}
