using SongSpiration.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SongSpiration.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> SearchUsersAsync(string criteria);
        Task DeleteUserAsync(int id);
        Task BanUserAsync(int id);
        Task DeletePinsForUserAsync(int id);
    }
}