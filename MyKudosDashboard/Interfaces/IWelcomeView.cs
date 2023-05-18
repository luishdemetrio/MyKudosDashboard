
using MyKudosDashboard.Models;

namespace MyKudosDashboard.Interfaces;

public interface IWelcomeView
{
    Task<string> GetUserPhoto(string userId);

    public delegate Task UpdateScoreCallBack(UserScore userScore);

    public UpdateScoreCallBack ScoreCallback { get; set; }

    public void RegisterForUserScoreUpdate(string userId);

    public delegate Task UpdateLikesCallBack(Like like);

    public UpdateLikesCallBack LikeCallback { get; set; }
    public UpdateLikesCallBack UndoLikeCallback { get; set; }


    public delegate Task UpdateKudosCallBack(KudosResponse kudos);

    public UpdateKudosCallBack KudosCallback { get; set; }


    public delegate Task CommentsCallBack(CommentsRequest like);

    public CommentsCallBack CommentsSentCallback { get; set; }
    public CommentsCallBack CommentsUpdatedCallback { get; set; }
    public CommentsCallBack CommentsDeletedCallback { get; set; }


}
