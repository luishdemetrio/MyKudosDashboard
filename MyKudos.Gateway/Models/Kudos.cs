namespace MyKudos.Gateway.Models;


public class Kudos {
    public string Id { get; set; }
    public string FromPersonId { get; set; }
    public string ToPersonId { get; set; }
    public string TitleId { get; set; }
    public string Message { get; set; }
    public DateTime Date { get; set; }
    public List<string> Likes { get; set; } = new();
    }



public record KudosRequest (
    Person From,
    Person To,
    Reward Title,
    string Message,
    DateTime SendOn
    );

public record KudosNotification 
(
    Person From,
    Person To,
    Reward Title,
    string Message,
    DateTime SendOn,
    string ManagerId
    
);
    
 


public record KudosResponse(
    string? Id,
    Person From,
    Person To,
    string Title,
    string Message,
    DateTime SendOn,
    IEnumerable<Person> Likes
    );

public record Reward(string Id, string Description);



//public record KudosNotification(
//    string? Id,
//    Person From,
//    Person To,
//    string Title,
//    string Message,
//    DateTime SendOn
//    );