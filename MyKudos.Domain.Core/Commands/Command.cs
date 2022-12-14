using MyKudos.Domain.Core.Events;


namespace MyKudos.Domain.Core.Commands;

public abstract class Command : Message
{
    public DateTime Timestamp { get; protected set; }

    protected Command()
    {
        Timestamp = DateTime.Now;
    }


}