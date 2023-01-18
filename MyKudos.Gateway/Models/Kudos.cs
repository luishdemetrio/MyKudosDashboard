namespace MyKudos.Gateway.Models;


public record Kudos(
    string Id,
    string From,
    string To,
    string TitleId,
    string Message,
    DateTime SendOn
    );



public record KudosRequest (
    Person From,
    Person To,
    Reward Title,
    string Message
    );


public record KudosResponse(
    string? Id,
    Person From,
    Person To,
    string Title,
    string Message,
    DateTime SendOn
    );

public record Reward(string Id, string Description);

//public record KudosNotification(
//    string? Id,
//    Person From,
//    Person To,
//    string Title,
//    string Message,
//    DateTime SendOn
//    );