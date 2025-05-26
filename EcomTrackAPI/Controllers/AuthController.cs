using Microsoft.AspNetCore.Mvc;
using EcomTrackAPI.Services;
using EcomTrackAPI.Models;

namespace EcomTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] Login login)
        {
            if (login == null) return Ok("El correo y la contraseña no pueden ser nulos");

            var sesion = AuthService.Login(login);

            return Ok(sesion);
        }
    }
}
