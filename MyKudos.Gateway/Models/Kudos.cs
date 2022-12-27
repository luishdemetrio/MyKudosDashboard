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
    string From,
    string To,
    string TitleId,
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

