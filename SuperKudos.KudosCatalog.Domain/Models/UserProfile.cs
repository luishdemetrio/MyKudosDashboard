
using System.ComponentModel.DataAnnotations;

namespace SuperKudos.KudosCatalog.Domain.Models;

public class UserProfile
{
    public Guid UserProfileId { get; set; }
    [MaxLength(60)]
    public string DisplayName { get; set; }
    public string? GivenName { get; set; }
    public string? Mail { get; set; }

    public string? Photo96x96 { get; set; }
    public string? Photo { get; set; }

    public Guid? ManagerId { get; set; }
}
