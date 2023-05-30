
namespace MyKudos.Kudos.Domain.Models;

public class Points
{
    public int PointsId { get; set; }
    public string ActionType { get; set; } = string.Empty;
    public int Score { get; set; }
}

public class UserPointScore
{
    public string UserId { get; set; }
    
    public int Score { get; set; }

    public int KudosSent { get; set; }
    public int KudosReceived { get; set; }

    public int LikesSent { get; set; }
    public int LikesReceived { get; set; }

    public int MessagesReceived { get; set; }
    public int MessagesSent { get; set; }

    public List<string> EarnedBagdes { get; set; } = new();
}


public class UserPoint
{
    public string UserId { get; set; }
    public int TotalPoints { get; set; }
}

