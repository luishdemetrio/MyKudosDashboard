namespace MyKudos.Gateway.Models;

public class UserScore
{
    public string UserId { get; set; }
    public int Score { get; set; }
    public int KudosSent { get; set; }
    public int KudosReceived { get; set; }
    public int LikesSent { get; set; }
    public int LikesReceived { get; set; }
    public int MessagesReceived { get; set; }
    public int MessagesSent { get; set; }
}
