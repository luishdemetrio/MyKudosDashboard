

namespace MyKudos.Dashboard.Domain.Models;

public record KudosLog(string FromPersonId, string ToPersonId,  string TitleId, string? Message, DateTime Date);