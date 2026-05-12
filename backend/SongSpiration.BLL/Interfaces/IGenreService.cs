using SongSpiration.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SongSpiration.BLL.Interfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<Genre>> GetAllGenresAsync();
    }
}