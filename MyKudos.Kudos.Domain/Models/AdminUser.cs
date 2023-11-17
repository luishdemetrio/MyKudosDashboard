
using System.ComponentModel.DataAnnotations.Schema;

namespace MyKudos.Kudos.Domain.Models;

public class AdminUser
{
    public int AdminUserId { get; set; }
    public Guid UserProfileId { get; set; }

    [ForeignKey("AdminUserId")]
    public UserProfile? Person { get; set; }
}
