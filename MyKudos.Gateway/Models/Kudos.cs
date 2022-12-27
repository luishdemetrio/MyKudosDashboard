namespace MyKudos.Gateway.Models;

public record Kudos (
    string id,
    string SentBy,
    string SentTo,
    string Title,
    string Message,
    DateTime SendOn
    );

