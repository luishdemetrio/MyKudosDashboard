
namespace MyKudos.Kudos.Domain.Models;

public class KudosLog
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string FromPersonId { get; set; }
    
    public string ToPersonId { get; set; }

    public string TitleId { get; set; }

    public string? Message { get; set; }

    public DateTime Date { get; set; }

    public List<string> Likes { get; set; } = new();
}


public record KudosNotification(
    Person From,
    Person To,
    Reward Title,
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

public record Reward(string Id, string Description);


public class GraphUser
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public string UserPrincipalName { get; set; }

}