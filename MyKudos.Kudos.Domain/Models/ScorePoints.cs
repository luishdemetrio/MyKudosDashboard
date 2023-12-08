

namespace MyKudos.Kudos.Domain.Models;

public class ScorePoints
{
    public int ScorePointsId { get; set; }
    public int KudosSent { get; set; }
    public int KudosReceived { get; set; }
    public int LikesSent { get; set; }
    public int LikesReceived { get; set; }
    public int CommentsSent { get; set; }
    public int CommentsReceived { get; set;}
}
