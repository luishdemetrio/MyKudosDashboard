using MyKudos.Domain.Core.Commands;

namespace MyKudos.Dashboard.Domain.Commands;

public abstract class SendKudosCommand : Command
{
    public string? FromPersonId { get; set; }

    public string? ToPersonId { get; set; }

    public string? TitleId { get; protected set; }

    public string? Message { get; protected set; }

    public DateTime? Date { get; set; }
}
