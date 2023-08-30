namespace MyKudos.Gateway.Models;

public class CommentsRequest
{
    public int Id { get; set; }
    public int KudosId { get; set; }
    public string FromPersonId { get; set; }
    public string ToPersonId { get; set; }
    public string Message { get; set; }
    public DateTime Date { get; set; }    
}


public class CommentsResponse
{
    public int Id { get; set; }
    public int KudosId { get; set; }
    public Person FromPerson { get; set; }
    public string Message { get; set; }
    public DateTime Date { get; set; }
    public List<Person> Likes { get; set; } = new();
}


