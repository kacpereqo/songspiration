using System;

namespace SongSpiration.Models;

public class Like
{
    public Guid UserId { get; set; }
    public Guid PinId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Pin Pin { get; set; } = null!;
}
