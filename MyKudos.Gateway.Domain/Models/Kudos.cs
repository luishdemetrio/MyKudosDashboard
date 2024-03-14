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




public record KudosMessage (int KudosId, string Message);

public class SendKudosRequest
{

    public int KudosId { get; set; }

    public Guid FromPersonId { get; set; }

    public List<Guid> ToPersonId { get; set; }

    public int RecognitionId { get; set; }

    public string? Message { get; set; }

    public DateTime Date { get; set; }
}

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
    List<Person> Receivers ,
    Reward Reward,
    string Message,
    DateTime SendOn,
    List<Guid> Recipients
    
);



public class KudosResponse
{
    public int Id { get; set; }
    public Person From { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public DateTime SendOn { get; set; }
    public List<Person> Likes { get; set; } = new();
    public List<int> Comments { get; set; }
    
    public List<Person> Receivers { get; set; } = new();

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