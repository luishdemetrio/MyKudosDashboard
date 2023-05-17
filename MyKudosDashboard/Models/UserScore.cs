namespace MyKudosDashboard.Models;

public class UserScore
{
    public string Id { get; set; }
    public int Score { get; set; }
    public int KudosSent { get; set; }
    public int KudosReceived { get; set; }
    public int LikesSent { get; set; }
    public int LikesReceived { get; set; }
    public int MessagesReceived { get; set; }
    public int MessagesSent { get; set; }
    public int? GroupOne { get; set; }
    public int? GroupTwo { get; set; }
    public int? GroupThree { get; set; }
    public int? GroupFour { get; set; }
    public int? GroupFive { get; set; }

    public int? GroupAll { get; set; }
}
