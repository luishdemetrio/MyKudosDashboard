
namespace MyKudos.Kudos.Domain.Models;

public class Points
{
    public int PointsId { get; set; }
    public string ActionType { get; set; } = string.Empty;
    public int Score { get; set; }
}

public class UserPointScore
{
    public Guid UserId { get; set; }
    
    public int Score { get; set; }

    public int KudosSent { get; set; }
    public int KudosReceived { get; set; }

    public int LikesSent { get; set; }
    public int LikesReceived { get; set; }

    public int MessagesReceived { get; set; }
    public int MessagesSent { get; set; }

    public List<UserBadge> EarnedBagdes { get; set; } = new();

    
}


public class UserPoint
{
    public Guid UserId { get; set; }
    public int TotalPoints { get; set; }
}

public class UserBadge
{
    public int UserBadgeId { get; set; }
    public string BadgeName { get; set; }
    public string BadgeDescription { get; set; }
}

public class BadgeRules
{
    public int BadgeRulesId { get; set; }
    public string Description { get; set; }
    public string ImageName { get; set; }
    public string ActionType { get; set; }
    public int Initial { get; set; }
    public int? Final { get; set; }

}
