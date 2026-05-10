namespace SongSpiration.BLL.DTOs;

public class UserProfileDto
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string? Bio { get; set; }
    
    // Statystyki profilu
    public int AddedPinsCount { get; set; }
    public int TotalLikesReceived { get; set; }
}
