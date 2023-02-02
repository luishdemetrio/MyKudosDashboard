namespace MyKudos.Gateway.Models;

public record LikeGateway(string KudosId, string PersonId);

public record LikeDTO(string KudosId, Person Person);
