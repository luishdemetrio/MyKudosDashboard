
namespace MyKudos.Kudos.Domain.Models;

public class KudosLog
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string? FromPersonId { get; set; }
    
    public string? ToPersonId { get; set; }

    public string? TitleId { get; set; }

    public string? Message { get; set; }

    public DateTime? Date { get; set; }
}
