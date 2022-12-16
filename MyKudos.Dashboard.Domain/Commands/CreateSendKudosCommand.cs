

namespace MyKudos.Dashboard.Domain.Commands;

public class CreateSendKudosCommand : SendKudosCommand
{
    public CreateSendKudosCommand(string? fromPersonId, string? toPersonId, string? titleId, string? message, DateTime? date)
    {
        FromPersonId = fromPersonId;
        ToPersonId  = toPersonId;
        TitleId = titleId;
        Message = message;
        Date = date;
    }
}
