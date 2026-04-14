using System;

namespace SongSpiration.Models;

public class PinGenre
{
    public Guid PinId { get; set; }
    public Guid GenreId { get; set; }

    // Navigation properties
    public virtual Pin Pin { get; set; } = null!;
    public virtual Genre Genre { get; set; } = null!;
}
