
namespace MyKudos.Gamification.Domain.Models;

public class UserScore
{
    public Guid Id { get; set; }     
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
