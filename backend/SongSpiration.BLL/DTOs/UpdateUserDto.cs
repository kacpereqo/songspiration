namespace SongSpiration.BLL.DTOs;

public class UpdateUserDto
{
    public required string DisplayName { get; set; }
    public required string Email { get; set; }
    public string? Bio { get; set; }
    public string? AvatarUrl { get; set; }
}