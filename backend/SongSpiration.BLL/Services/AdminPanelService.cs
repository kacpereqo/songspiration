using SongSpiration.BLL.Interfaces;
using SongSpiration.DAL.Interfaces;
using SongSpiration.Models;
using SongSpiration.BLL.DTOs;

namespace SongSpiration.BLL.Services
{
    public class AdminPanelService : IAdminPanelService
    {
        private readonly IUserRepository _userRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IPinRepository _pinRepository;

        public AdminPanelService(IUserRepository userRepository, IGenreRepository genreRepository, IPinRepository pinRepository)
        {
            _userRepository = userRepository;
            _genreRepository = genreRepository;
            _pinRepository = pinRepository;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> CreateUserAsync(UserCreateDto userCreateDto)
        {
            var user = new User
            {
                Username = userCreateDto.Username,
                Email = userCreateDto.Email,
                PasswordHash = userCreateDto.PasswordHash,
                Role = userCreateDto.Role
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveAsync();

            return user;
        }

        public async Task UpdateUserAsync(int userId, UserUpdateDto userUpdateDto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.Username = userUpdateDto.Username;
            user.Email = userUpdateDto.Email;
            user.PasswordHash = userUpdateDto.PasswordHash;
            user.Role = userUpdateDto.Role;

            _userRepository.Update(user);
            await _userRepository.SaveAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            _userRepository.Delete(user);
            await _userRepository.SaveAsync();
        }

        public async Task<List<Genre>> GetAllGenresAsync()
        {
            return await _genreRepository.GetAllAsync();
        }

        public async Task<Genre> GetGenreByIdAsync(int genreId)
        {
            return await _genreRepository.GetByIdAsync(genreId);
        }

        public async Task<Genre> CreateGenreAsync(GenreCreateDto genreCreateDto)
        {
            var genre = new Genre
            {
                Name = genreCreateDto.Name
            };

            await _genreRepository.AddAsync(genre);
            await _genreRepository.SaveAsync();

            return genre;
        }

        public async Task UpdateGenreAsync(int genreId, GenreCreateDto genreCreateDto)
        {
            var genre = await _genreRepository.GetByIdAsync(genreId);
            if (genre == null)
            {
                throw new Exception("Genre not found");
            }

            genre.Name = genreCreateDto.Name;

            _genreRepository.Update(genre);
            await _genreRepository.SaveAsync();
        }

        public async Task DeleteGenreAsync(int genreId)
        {
            var genre = await _genreRepository.GetByIdAsync(genreId);
            if (genre == null)
            {
                throw new Exception("Genre not found");
            }

            _genreRepository.Delete(genre);
            await _genreRepository.SaveAsync();
        }

        public async Task<List<User>> SearchUsersAsync(string searchTerm)
        {
            return await _userRepository.SearchAsync(searchTerm);
        }

        public async Task BanUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.IsBanned = true;

            _userRepository.Update(user);
            await _userRepository.SaveAsync();
        }

        public async Task DeletePinsForUserAsync(int userId)
        {
            var pins = await _pinRepository.GetPinsByUserIdAsync(userId);
            foreach (var pin in pins)
            {
                _pinRepository.Delete(pin);
            }
            await _pinRepository.SaveAsync();
        }
    }
}