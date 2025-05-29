using EcomTrackAPI.Models;
using EcomTrackAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcomTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DireccionController : ControllerBase
    {
        [HttpGet("ListaDirecciones")]
        public IActionResult ListaDirecciones()
        {
            var respuesta = DireccionService.ListaDirecciones();

            if (!respuesta.Resultado)
            {
                return BadRequest(respuesta);
            }

            return Ok(respuesta);
        }

        [HttpDelete("EliminarDireccion")]
        public IActionResult EliminarDireccion(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El ID de la direccion no es válido",
                    Datos = null
                });
            }

            var respuesta = DireccionService.EliminarDireccion(id);

            if (!respuesta.Resultado)
            {
                return BadRequest(respuesta);
            }

            return Ok(respuesta);
        }

        [HttpPost("GuardarDireccion")]
        public IActionResult GuardarDireccion(Direccion direccion)
        {
            var respuesta = DireccionService.GuardarDireccion(direccion);

            if (!respuesta.Resultado)
            {
                return BadRequest(respuesta);
            }

            return Ok(respuesta);
        }

        [HttpPost("EditarDireccion")]
        public IActionResult EditarDireccion(Direccion direccion)
        {
            var respuesta = DireccionService.EditarDireccion(direccion);

            if (!respuesta.Resultado)
            {
                return BadRequest(respuesta);
            }

            return Ok(respuesta);
        }
    }
}
