namespace SongSpiration.BLL.DTOs;

public class UpdateUserDto
{
    public string DisplayName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string? Bio { get; set; }
}
