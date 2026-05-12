using SongSpiration.BLL.Interfaces;
using SongSpiration.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SongSpiration.BLL.Services
{
    public class GenreService : IGenreService
    {
        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            // Mockowe dane - w przyszłości pobierać z bazy danych
            return new List<Genre>
            {
                new Genre { Id = new System.Guid("11111111-1111-1111-1111-111111111111"), Name = "Rock", Slug = "rock" },
                new Genre { Id = new System.Guid("22222222-2222-2222-2222-222222222222"), Name = "Pop", Slug = "pop" },
                new Genre { Id = new System.Guid("33333333-3333-3333-3333-333333333333"), Name = "Jazz", Slug = "jazz" },
                new Genre { Id = new System.Guid("44444444-4444-4444-4444-444444444444"), Name = "Classical", Slug = "classical" },
                new Genre { Id = new System.Guid("55555555-5555-5555-5555-555555555555"), Name = "Hip-Hop", Slug = "hiphop" }
            };
        }
    }
}