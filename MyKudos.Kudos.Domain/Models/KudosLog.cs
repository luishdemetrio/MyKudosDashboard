
using System.ComponentModel.DataAnnotations;

namespace MyKudos.Kudos.Domain.Models;

public class Kudos
{
    
    public int KudosId { get; set; } 

    public string FromPersonId { get; set; }
    
    public string ToPersonId { get; set; }

    public int RecognitionId { get; set; }

    public string? Message { get; set; }

    public DateTime Date { get; set; }

    public List<KudosLike> Likes { get; set; } = new();

    public List<Comments> Comments { get; set; } = new();
}


public class Comments
{
    public int CommentsId { get; set; } 
    public int KudosId { get; set; }
    public string FromPersonId { get; set; }
    public string Message { get; set; }
    public DateTime Date { get; set; }
    public List<CommentsLikes> Likes { get; set; } = new();

}

public class CommentsLikes
{
    public int Id { get; set; }
    public int CommentsId { get; set; }
    public string FromPersonId { get; set; }
}

public class KudosLike
{
    public int KudosLikeId { get; set; }
    public int KudosId { get; set; }
    public string PersonId { get; set; }
}


public record KudosNotification(
    Person From,
    Person To,
    Reward Reward,
    string Message,
    DateTime SendOn,
    string ManagerId
    );


public class Person
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Photo { get; set; }

    
}

public record Reward(int Id, string Title);


public class GraphUser
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public string UserPrincipalName { get; set; }

}