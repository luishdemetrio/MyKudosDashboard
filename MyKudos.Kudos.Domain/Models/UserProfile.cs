﻿
using System.ComponentModel.DataAnnotations;

namespace MyKudos.Kudos.Domain.Models;

public class UserProfile
{
    public Guid UserProfileId { get; set; }
    [MaxLength(60)]
    public string DisplayName { get; set; }
    
    public string? Photo { get; set; }
}
