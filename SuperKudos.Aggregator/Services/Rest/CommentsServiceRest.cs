using MyKudos.Communication.Helper.Interfaces;
using SuperKudos.Aggregator.Interfaces;
using SuperKudos.KudosCatalog.Domain.Models;


namespace SuperKudos.Aggregator.Services;

public class CommentsServiceRest: ICommentsService
{

    private readonly string _kudosServiceUrl;
    
    private IRestClientHelper _restClientHelper;

    private readonly ILogger<CommentsServiceRest> _logger;

    public CommentsServiceRest(IConfiguration config, ILogger<CommentsServiceRest> log, IRestClientHelper clientHelper)
    {
        _kudosServiceUrl = config["kudosServiceUrl"];
        _logger = log;
        _restClientHelper = clientHelper;
    }

    public async Task<bool> DeleteComments(int kudosId, int commentId)
    {
        bool result = false ;
        
        try
        {
            result = await _restClientHelper.SendApiData<string, bool>($"{_kudosServiceUrl}Comments?kudosId={kudosId}&commentId={commentId}",
                                                                       HttpMethod.Delete, body: null);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing DeleteComments: {ex.Message}");
        }

        return result;

    }

    public async Task<IEnumerable<Comments>> GetComments(int kudosId)
    {
        List<Comments> result = new();

        try
        {

            var comments = await _restClientHelper.GetApiData<IEnumerable<Comments>>($"{_kudosServiceUrl}Comments?kudosId={kudosId}");
            result = comments.ToList();

        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing GetComments: {ex.Message}");
        }

        return result;

    }
      
    public async Task<int> SendCommentsAsync(Comments comment)
    {
        int result =0;

        try
        {
            result = await _restClientHelper.SendApiData<Comments, int>($"{_kudosServiceUrl}Comments", HttpMethod.Post, comment);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing SendCommentsAsync: {ex.Message}");
        }

        return result;

       
    }

    public async Task<bool> UpdateComments(Comments comments)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<Comments, bool>($"{_kudosServiceUrl}Comments", HttpMethod.Put, comments);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing UpdateComments: {ex.Message}");
        }

        return result;

    }

    public async Task<bool> LikeCommentAsync(SendLike like)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<SendLike, bool>($"{_kudosServiceUrl}likecomment", HttpMethod.Post, like);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing LikeCommentAsync: {ex.Message}");
        }

        return result;

    }

    public async Task<bool> UndoLikeCommentAsync(SendLike like)
    {
        bool result = false;

        try
        {
            result = await _restClientHelper.SendApiData<SendLike, bool>($"{_kudosServiceUrl}likecomment", HttpMethod.Delete, like);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error processing UndoLikeComment: {ex.Message}");
        }

        return result;

       
    }
}
