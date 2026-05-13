namespace SongSpiration.BLL.DTOs;

public class UserProfileDto
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string? Bio { get; set; }
    
    public int AddedPinsCount { get; set; }
    public int TotalLikesReceived { get; set; }
}