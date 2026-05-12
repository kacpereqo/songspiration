using SongSpiration.BLL.Interfaces;
using SongSpiration.DAL.Interfaces;
using SongSpiration.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SongSpiration.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> SearchUsersAsync(string criteria)
        {
            return await _userRepository.SearchUsersAsync(criteria);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteUserAsync(id);
        }

        public async Task BanUserAsync(int id)
        {
            await _userRepository.BanUserAsync(id);
        }

        public async Task DeletePinsForUserAsync(int id)
        {
            await _userRepository.DeletePinsForUserAsync(id);
        }
    }
}