namespace MyKudos.Gateway.Models;

public record Like(string KudosId, string PersonId);

public record LikeDTO(string KudosId, Person Person);
