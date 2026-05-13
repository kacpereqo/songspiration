using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SongSpiration.Models;

namespace SongSpiration.DAL.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);
    void Update(User user);
    void Delete(User user);
    Task<int> SaveChangesAsync();
    Task<IEnumerable<User>> SearchUsersAsync(string criteria);
    Task BanUserAsync(int userId);
    Task DeleteUserPinsAsync(int userId);
}
