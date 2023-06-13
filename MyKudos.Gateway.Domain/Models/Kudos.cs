namespace MyKudos.Gateway.Domain.Models;


//public class Kudos {
//    public int Id { get; set; }
//    public string FromPersonId { get; set; }
//    public string ToPersonId { get; set; }
//    public string TitleId { get; set; }
//    public string Message { get; set; }
//    public DateTime Date { get; set; }
//    public List<string> Likes { get; set; } = new();
//    public List<int> Comments { get; set; } = new();
//}



public record KudosRequest(
    Person From,
    Person To,
    Reward Reward,
    string Message,
    DateTime SendOn
    );

public record KudosNotification 
(
    Person From,
    Person To,
    Reward Reward,
    string Message,
    DateTime SendOn,
    Guid ManagerId
    
);



public class KudosResponse
{
    public int Id { get; set; }
    public Person From { get; set; }
    public Person To { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public DateTime SendOn { get; set; }
    public List<Person> Likes { get; set; } = new();
    public List<int> Comments { get; set; } 



}

public record Reward(int Id, string Title);



//public record KudosNotification(
//    string? Id,
//    Person From,
//    Person To,
//    string Title,
//    string Message,
//    DateTime SendOn
//    );