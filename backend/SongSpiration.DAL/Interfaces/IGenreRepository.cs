using ModelsGenre = SongSpiration.Models.Genre;
using EntitiesGenre = SongSpiration.Models.Entities.Genre;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SongSpiration.DAL.Interfaces
{
    public interface IGenreRepository
    {
        Task<IEnumerable<EntitiesGenre>> GetAllAsync();
        Task<EntitiesGenre> GetByIdAsync(Guid id);
        Task AddAsync(EntitiesGenre genre);
        void Update(EntitiesGenre genre);
        void Delete(EntitiesGenre genre);
        Task SaveAsync();
    }
}
