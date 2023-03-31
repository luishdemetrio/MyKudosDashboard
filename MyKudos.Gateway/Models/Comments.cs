namespace MyKudos.Gateway.Models;

public class CommentsRequest
{
    public string? Id { get; set; }
    public string KudosId { get; set; }
    public string FromPersonId { get; set; }
    public string Message { get; set; }
    public DateTime Date { get; set; }    
}


public class CommentsResponse
{
    public string Id { get; set; }
    public string KudosId { get; set; }
    public Person FromPerson { get; set; }
    public string Message { get; set; }
    public DateTime Date { get; set; }
    public List<Person> Likes { get; set; } = new();
}


