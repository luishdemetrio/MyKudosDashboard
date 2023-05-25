namespace MyKudos.Gateway.Domain.Models;


public record LikeGateway(int KudosId, Person FromPerson, string ToPersonId);


public record LikeCommentGateway(int KudosId, Person FromPerson);

public record LikeDTO(int KudosId, Person Person);

public record LikeMessage(int MessageId, Person Person);


