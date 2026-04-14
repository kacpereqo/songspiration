using System;
using System.Collections.Generic;

namespace SongSpiration.Models;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string? Bio { get; set; }
    public string Roles { get; set; } = "User";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLogin { get; set; }
    public bool IsEmailVerified { get; set; }

    // Navigation properties
    public virtual ICollection<Pin> Pins { get; set; } = new List<Pin>();
    public virtual ICollection<AuthToken> AuthTokens { get; set; } = new List<AuthToken>();
    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
}
