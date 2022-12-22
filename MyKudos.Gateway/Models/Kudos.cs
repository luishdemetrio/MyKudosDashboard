namespace MyKudos.Gateway.Models;

public record Kudos (
    Guid id,
    string SentBy,
    string SentTo,
    string Title,
    string Message,
    DateTime SendOn
    );

