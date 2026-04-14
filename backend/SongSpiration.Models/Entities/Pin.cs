using System;
using System.Collections.Generic;

namespace SongSpiration.Models;

public class Pin
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Instrument Instrument { get; set; }
    public PinVisibility Visibility { get; set; }

    public string GpFileLocationOrBlob { get; set; } = string.Empty;
    public string Filename { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public long Size { get; set; }
    public string Checksum { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual User Owner { get; set; } = null!;
    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
    public virtual ICollection<PinGenre> PinGenres { get; set; } = new List<PinGenre>();
}
