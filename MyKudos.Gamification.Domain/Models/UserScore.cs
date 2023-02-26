﻿
namespace MyKudos.Gamification.Domain.Models;

public class UserScore
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserId { get; set; }
    public int Score { get; set; }
    public int KudosSent { get; set; }
    public int KudosReceived { get; set; }
    public int LikesSent { get; set; }
    public int LikesReceived { get; set; }
}