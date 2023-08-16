namespace MyKudos.Gateway.Domain.Models;

public class CommentsRequest
{
    public int Id { get; set; }
    public int KudosId { get; set; }
    public Guid FromPersonId { get; set; }
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


