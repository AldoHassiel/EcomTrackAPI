using Microsoft.AspNetCore.Mvc;
using EcomTrackAPI.Models;
using EcomTrackAPI.Services;

namespace EcomTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpPost("RegistrarAdmin")]
        public IActionResult RegistrarAdmin([FromBody] AdminRegistro admin)
        {
            if (admin == null) return BadRequest("El administrador no puede ser nulo.");

            var resultado = AdminServices.RegistrarAdmin(admin);

            if (resultado.Resultado)
            {
                return Ok(new { Resultado = true, Mensaje = "Administrador registrado correctamente", IdAdmin = resultado.IdUsuario });
            }

            return BadRequest(new { Resultado = false, Mensaje = resultado.Mensaje });
        }

        [HttpPut("ActualizarAdmin")]
        public IActionResult ActualizarAdmin([FromBody] Admin admin)
        {
            if (admin == null) return BadRequest("El administrador no puede ser nulo.");

            var resultado = AdminServices.ActualizarAdmin(admin);

            if (resultado.Resultado)
            {
                return Ok(new { Resultado = true, Mensaje = "Administrador actualizado correctamente" });
            }

            return BadRequest(new { Resultado = false, Mensaje = resultado.Mensaje });
        }

        [HttpDelete("EliminarAdmin")]
        public IActionResult EliminarAdmin(int id)
        {
            if (id <= 0) return BadRequest("El ID del administrador no es válido.");

            var resultado = AdminServices.EliminarAdmin(id);

            if (resultado.Resultado)
            {
                return Ok(new { Resultado = true, Mensaje = "Administrador eliminado correctamente" });
            }

            return BadRequest(new { Resultado = false, Mensaje = resultado.Mensaje });
        }

        [HttpGet("ObtenerAdminPorId")]
        public IActionResult ObtenerAdminPorId(int id)
        {
            if (id <= 0) return BadRequest("El ID del administrador no es válido.");

            var admin = AdminServices.ObtenerAdminPorId(id);

            if (admin == null)
            {
                return NotFound(new { Resultado = false, Mensaje = "Administrador no encontrado" });
            }

            return Ok(new { Resultado = true, admin });
        }

        [HttpGet("ObtenerAdmins")]
        public IActionResult ObtenerAdmins()
        {
            var admins = AdminServices.ObtenerAdmins();

            if (admins == null || admins.Count == 0)
            {
                return NotFound(new { Mensaje = "No se encontraron administradores" });
            }

            return Ok(new { Resultado = true, admins });
        }
    }
}
