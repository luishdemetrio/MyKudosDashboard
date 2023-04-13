namespace MyKudosDashboard.Models;

//public record Like(string KudosId, Person FromPerson, string ToPersonId);


public record Like(string KudosId, Person FromPerson, string ToPersonId);

public record LikeComment(string KudosId, Person FromPerson);