using Microsoft.AspNetCore.Mvc;
using SongSpiration.BLL.DTOs;
using SongSpiration.BLL.Interfaces;

namespace SongSpiration.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterUserDto registerDto)
        {
            try
            {
                var response = await _userService.RegisterAsync(registerDto);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Wystąpił błąd podczas rejestracji." });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            try
            {
                var response = await _userService.LoginAsync(loginDto);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Wystąpił błąd podczas logowania." });
            }
        }

        [HttpGet("profile/{id}")]
        public async Task<ActionResult<UserProfileDto>> GetProfile(Guid id)
        {
            // POPRAWKA: Zmiana nazwy na GetUserProfileAsync
            var profile = await _userService.GetUserProfileAsync(id);
            if (profile == null)
            {
                return NotFound(new { message = "Użytkownik nie został znaleziony." });
            }
            return Ok(profile);
        }

        [HttpPut("profile/{id}")]
        public async Task<IActionResult> UpdateProfile(Guid id, UpdateUserDto updateDto)
        {
            try
            {
                // POPRAWKA: Serwis zwraca bool (true/false)
                var success = await _userService.UpdateProfileAsync(id, updateDto);
                
                if (!success)
                {
                    return NotFound(new { message = "Użytkownik nie istnieje lub nie udało się zaktualizować profilu." });
                }

                return Ok(new { message = "Profil zaktualizowany pomyślnie." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Błąd podczas aktualizacji profilu." });
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            var result = await _userService.DeleteAccountAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            try
            {
                await _userService.ForgotPasswordAsync(dto);
                return Ok(new { message = "Jeśli email istnieje w systemie, wysłaliśmy link do resetu hasła." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Wystąpił błąd podczas przetwarzania żądania." });
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            try
            {
                await _userService.ResetPasswordAsync(dto);
                return Ok(new { message = "Hasło zostało pomyślnie zmienione." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Wystąpił błąd podczas resetowania hasła." });
            }
        }
    }
}