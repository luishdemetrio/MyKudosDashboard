

namespace MyKudos.Dashboard.Domain.Commands;

public class CreateSendKudosCommand : SendKudosCommand
{
    public CreateSendKudosCommand(string? personId, string? titleId, string? message)
    {
        PersonId = personId;
        TitleId = titleId;
        Message = message;
    }
}
