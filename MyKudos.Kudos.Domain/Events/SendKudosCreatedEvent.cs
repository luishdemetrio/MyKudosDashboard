
using MyKudos.Domain.Core.Events;

namespace MyKudos.Kudos.Domain.Events;

public class SendKudosCreatedEvent : Event
{
    public string? FromPersonId { get; set; }

    public string? ToPersonId { get; set; }

    public string? TitleId { get; set; }

    public string? Message { get; set; }

    public DateTime Date { get; set; }

    public SendKudosCreatedEvent(string fromPersonId, string ToPerson, string titleId, string? message, DateTime date)
    {
        FromPersonId = fromPersonId;
        ToPersonId = ToPersonId;
        TitleId = titleId;
        Message = message;
        Date = date;
    }
}
