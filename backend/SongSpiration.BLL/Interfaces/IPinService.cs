namespace SongSpiration.BLL.Interfaces
{
    public interface IPinService
    {
        Task<List<Pin>> GetAllPinsAsync();
        Task<Pin> GetPinByIdAsync(int id);
        Task<Pin> CreatePinAsync(PinCreateDto pinCreateDto);
        Task<Pin> UpdatePinAsync(int id, PinCreateDto pinCreateDto);
        Task DeletePinAsync(int id);
    }
}