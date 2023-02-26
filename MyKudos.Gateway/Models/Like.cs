namespace MyKudos.Gateway.Models;

public record LikeGateway(string KudosId, string FromPersonId, string ToPersonId);

public record LikeDTO(string KudosId, Person Person);
