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
            if (string.IsNullOrWhiteSpace(admin.Correo))
            {
                return BadRequest(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El correo es obligatorio",
                    Datos = null
                });
            }

            if (string.IsNullOrWhiteSpace(admin.Pwd))
            {
                return BadRequest(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "La contraseña es obligatoria",
                    Datos = null
                });
            }

            if (string.IsNullOrWhiteSpace(admin.Nombre))
            {
                return BadRequest(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El nombre es obligatorio",
                    Datos = null
                });
            }

            if (string.IsNullOrWhiteSpace(admin.Telefono))
            {
                return BadRequest(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El teléfono es obligatorio",
                    Datos = null
                });
            }

            var respuesta = AdminServices.RegistrarAdmin(admin);

            if (!respuesta.Resultado)
            {
                return BadRequest(respuesta);
            }

            return Ok(respuesta);
        }

        [HttpPut("ActualizarAdmin")]
        public IActionResult ActualizarAdmin([FromBody] Admin admin)
        {
            if (admin == null)
            {
                return BadRequest(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El administrador no puede ser nulo",
                    Datos = null
                });
            }

            if (string.IsNullOrWhiteSpace(admin.Correo))
            {
                return BadRequest(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El correo es obligatorio",
                    Datos = null
                });
            }

            if (string.IsNullOrWhiteSpace(admin.Pwd))
            {
                return BadRequest(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "La contraseña es obligatoria",
                    Datos = null
                });
            }

            if (string.IsNullOrWhiteSpace(admin.Nombre))
            {
                return BadRequest(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El nombre es obligatorio",
                    Datos = null
                });
            }

            if (string.IsNullOrWhiteSpace(admin.Telefono))
            {
                return BadRequest(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El teléfono es obligatorio",
                    Datos = null
                });
            }

            if (admin.Id <= 0)
            {
                return BadRequest(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El ID del administrador es obligatorio",
                    Datos = null
                });
            }

            var respuesta = AdminServices.ActualizarAdmin(admin);

            if (!respuesta.Resultado)
            {
                return BadRequest(respuesta);
            }
            return Ok(respuesta);
        }

        [HttpDelete("EliminarAdmin")]
        public IActionResult EliminarAdmin(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El ID del administrador es invalido",
                    Datos = null
                });
            }

            var respuesta = AdminServices.EliminarAdmin(id);

            if (!respuesta.Resultado)
            {
                return BadRequest(respuesta);
            }

            return Ok(respuesta);
        }

        [HttpGet("ObtenerAdmin")]
        public IActionResult ObtenerUnAdmin(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El ID del administrador es obligatorio",
                    Datos = null
                });
            }
            var respuesta = AdminServices.ObtenerUnAdmin(id);

            if (!respuesta.Resultado)
            {
                return BadRequest(respuesta);
            }

            return Ok(respuesta);
        }

        [HttpGet("ObtenerAdmins")]
        public IActionResult ObtenerAdmins()
        {
            var respuesta = AdminServices.ObtenerAdmins();

            if (!respuesta.Resultado)
            {
                return BadRequest(respuesta);
            }

            return Ok(respuesta);
        }
    }
}
