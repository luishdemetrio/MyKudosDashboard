
namespace MyKudos.Kudos.Domain.Models;

public class UserProfile
{
    public Guid UserProfileId { get; set; }
    public string DisplayName { get; set; }
    
    public string? Photo { get; set; }
}
