namespace MyKudos.Gateway.Domain.Models;

public class UserScore
{
    public int UserScoreId { get; set; }
    public Guid UserId { get; set; }
    public int Score { get; set; }
    public int KudosSent { get; set; }
    public int KudosReceived { get; set; }
    public int LikesSent { get; set; }
    public int LikesReceived { get; set; }
    public int MessagesReceived { get; set; }
    public int MessagesSent { get; set; }

    public int? GroupOne { get; set; } = 0;
    public int? GroupTwo { get; set; } = 0;
    public int? GroupThree { get; set; } = 0;
    public int? GroupFour { get; set; } = 0;
    public int? GroupFive { get; set; } = 0;

    public int? GroupAll { get; set; } = 0;
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
