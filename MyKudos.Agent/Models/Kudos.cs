namespace MyKudos.Agent.Models;

public record Kudos(
    string Id,
    string From,
    string To,
    string Title,
    string Message,
    DateTime SendOn
    );
