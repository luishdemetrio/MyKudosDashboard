﻿namespace MyKudos.Gateway.Domain.Models;


public record LikeGateway(int KudosId, Person FromPerson);

public record SendLikeGateway(int KudosId, Guid UserProfileId);


public record LikeCommentGateway(int KudosId, Person FromPerson);

public record LikeDTO(int KudosId, Person Person);

public record LikeMessage(int MessageId, Person Person);



