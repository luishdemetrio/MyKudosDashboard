using MyKudos.Domain.Core.Commands;

namespace MyKudos.Dashboard.Domain.Commands;

public abstract class SendKudosCommand : Command
{
    public string? PersonId { get; protected set; }

    public string? TitleId { get; protected set; }

    public string? Message { get; protected set; }
}
