using Microsoft.AspNetCore.Mvc;
using SongSpiration.BLL.Services;
using SongSpiration.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SongSpiration.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminPanelController : ControllerBase
    {
        private readonly AdminPanelService _adminPanelService;

        public AdminPanelController(AdminPanelService adminPanelService)
        {
            _adminPanelService = adminPanelService;
        }

        [HttpGet("genres")]
        public async Task<ActionResult<List<Genre>>> GetAllGenresAsync()
        {
            var genres = await _adminPanelService.GetAllGenresAsync();
            return Ok(genres);
        }

        [HttpGet("genres/{id}")]
        public async Task<ActionResult<Genre>> GetGenreByIdAsync(int id)
        {
            var genre = await _adminPanelService.GetGenreByIdAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            return Ok(genre);
        }

        [HttpPost("genres")]
        public async Task<ActionResult<Genre>> CreateGenreAsync([FromBody] GenreCreateDto genreCreateDto)
        {
            var genre = await _adminPanelService.CreateGenreAsync(genreCreateDto);
            return CreatedAtAction(nameof(GetGenreByIdAsync), new { id = genre.Id }, genre);
        }

        [HttpPut("genres/{id}")]
        public async Task<ActionResult<Genre>> UpdateGenreAsync(int id, [FromBody] GenreCreateDto genreCreateDto)
        {
            var genre = await _adminPanelService.UpdateGenreAsync(id, genreCreateDto);
            if (genre == null)
            {
                return NotFound();
            }
            return Ok(genre);
        }

        [HttpDelete("genres/{id}")]
        public async Task<ActionResult> DeleteGenreAsync(int id)
        {
            await _adminPanelService.DeleteGenreAsync(id);
            return NoContent();
        }

        [HttpGet("users")]
        public async Task<ActionResult<List<User>>> SearchUsersAsync([FromQuery] string criteria)
        {
            var users = await _adminPanelService.SearchUsersAsync(criteria);
            return Ok(users);
        }

        [HttpDelete("users/{id}")]
        public async Task<ActionResult> DeleteUserAsync(int id)
        {
            await _adminPanelService.DeleteUserAsync(id);
            return NoContent();
        }

        [HttpPut("users/{id}/ban")]
        public async Task<ActionResult> BanUserAsync(int id)
        {
            await _adminPanelService.BanUserAsync(id);
            return NoContent();
        }

        [HttpDelete("users/{id}/pins")]
        public async Task<ActionResult> DeletePinsForUserAsync(int id)
        {
            await _adminPanelService.DeletePinsForUserAsync(id);
            return NoContent();
        }
    }
}