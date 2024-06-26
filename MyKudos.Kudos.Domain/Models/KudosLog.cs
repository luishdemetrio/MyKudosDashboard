﻿
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyKudos.Kudos.Domain.Models;

public record KudosMessage(int KudosId, string Message);

public class Kudos
{
    
    public int KudosId { get; set; } 

    public Guid FromPersonId { get; set; }
    
    //public Guid ToPersonId { get; set; }

    public int RecognitionId { get; set; }

    public string? Message { get; set; }

    public DateTime Date { get; set; }

    public List<KudosLike> Likes { get; set; } = new();

    public List<Comments> Comments { get; set; } = new();

    [ForeignKey("FromPersonId")]
    public UserProfile? UserFrom { get; set; } 

    //[ForeignKey("ToPersonId")]
    //public UserProfile? UserTo { get; set; } 

    [ForeignKey("RecognitionId")]
    public Recognition? Recognition { get; set; }

    [ForeignKey("KudosId")]
    public List<KudosReceiver> Recognized { get; set; } = new();

    public int SentOnYear { get; set; }

}

public class KudosReceiver
{
    public int KudosReceiverId { get; set; }

    public int KudosId { get; set; }

    public Guid ToPersonId { get; set; }

    [ForeignKey("ToPersonId")]
    public UserProfile? Person { get; set; }
}

    public class Comments
{
    public int CommentsId { get; set; } 
    public int KudosId { get; set; }
    public Guid FromPersonId { get; set; }
    public string Message { get; set; }
    public DateTime Date { get; set; }
    public List<CommentsLikes> Likes { get; set; } = new();

    [ForeignKey("FromPersonId")]
    public UserProfile? UserFrom { get; set; }

}

public class CommentsLikes
{
    public int Id { get; set; }
    public int CommentsId { get; set; }
    public Guid FromPersonId { get; set; }

    [ForeignKey("FromPersonId")]
    public UserProfile Person { get; set; }
}

public class KudosLike
{
    public int KudosLikeId { get; set; }
    public int KudosId { get; set; }
    public Guid PersonId { get; set; }

    [ForeignKey("PersonId")]
    public UserProfile? Person { get; set; }
}


public record KudosNotification(
    Person From,
    List<Person> Receivers,
    Reward Reward,
    string Message,
    DateTime SendOn,
    Guid ManagerId
    );


public class Person
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Photo { get; set; }

    
}

public record Reward(int Id, string Title);


public class GraphUser
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; }
    public string UserPrincipalName { get; set; }

}