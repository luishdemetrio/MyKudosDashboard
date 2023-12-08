
namespace MyKudos.Gateway.Domain.Models;

public class Points
{
    public int ScorePointsId { get; set; }

    public int KudosSent { get; set; }
    public int KudosReceived { get; set; }

    public int LikesSent { get; set; }
    public int LikesReceived { get; set; }

    public int CommentsReceived { get; set; }
    public int CommentsSent { get; set; }

}

