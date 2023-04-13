using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface IGatewayService
{
    Task<IEnumerable<RecognitionViewModel>> GetRecognitionsAsync();

    Task<string> SendKudos(KudosRequest kudos);

    Task<IEnumerable<KudosResponse>> GetKudos();

    Task<IEnumerable<UserViewModel>> GetUsers(string name);

    Task<string> GetUserPhoto(string userid);

    Task<bool> Like(Like like);
    Task<bool> UndoLike(Like like);

    Task<bool> LikeComment(LikeComment like);
    Task<bool> UndoLikeComment(LikeComment like);

    Task<UserScore> GetUserScoreAsync(string pUserId);

    Task<IEnumerable<TopContributors>> GetTopContributors();

    Task<string> SendCommentsAsync(CommentsRequest comment);

    Task<IEnumerable<CommentsResponse>> GetComments(string kudosId);

    Task<bool> UpdateComments(CommentsRequest comments);

    Task<bool> DeleteComments(CommentsRequest comments);

}
