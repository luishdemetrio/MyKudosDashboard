using MyKudos.Domain.Core.Events;


namespace MyKudos.Dashboard.Domain.Events;

public class SendKudosCreatedEvent : Event
{
    public string PersonId { get; set; }

    public string TitleId { get; set; }

    public string? Message { get; set; }

    public SendKudosCreatedEvent(string personId, string titleId, string? message)
    {
        PersonId = personId;
        TitleId = titleId;
        Message = message;
    }
}
