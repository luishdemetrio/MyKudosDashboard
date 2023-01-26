namespace MyKudos.Gateway.Models;


public record Kudos(
    string Id,
    string From,
    string To,
    string TitleId,
    string Message,
    DateTime SendOn,
    IEnumerable<string> Likes
    );



public record KudosRequest (
    Person From,
    Person To,
    Reward Title,
    string Message,
    DateTime SendOn
    );

public record KudosNotification 
(
    Person From,
    Person To,
    Reward Title,
    string Message,
    DateTime SendOn,
    string ManagerId
    
);
    
    


public record KudosResponse(
    string? Id,
    Person From,
    Person To,
    string Title,
    string Message,
    DateTime SendOn,
    IEnumerable<Person> Likes
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