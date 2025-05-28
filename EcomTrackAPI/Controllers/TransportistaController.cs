using EcomTrackAPI.Models;
using EcomTrackAPI.Services;
using Microsoft.AspNetCore.Mvc;


namespace EcomTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransportistaController : ControllerBase
    {
        [HttpGet("ObtenerTransportista")]
        public IActionResult ObtenerTransportista()
        {
            var respuestaAPI = TransportistaService.ObtenerTransportista();

            return Ok(respuestaAPI);
        }

        [HttpPost("GuardarTransportista")]
        public IActionResult GuardarTransportista([FromBody] Transportista transportista)
        {
            if (transportista == null)
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El transportista no puede ser nulo",
                    Datos = null
                });
            }

            if (string.IsNullOrEmpty(transportista.Nombre) || string.IsNullOrEmpty(transportista.Correo) || string.IsNullOrEmpty(transportista.Telefono))
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "Los campos Nombre, Correo y Telefono son obligatorios",
                    Datos = null
                });
            }

            var respuestaAPI = TransportistaService.GuardarTransportista(transportista);

            return Ok(respuestaAPI);
        }

        [HttpPut("ActualizarTransportista")]
        public IActionResult ActualizarTransportista([FromBody] Transportista transportista)
        {
            if (transportista == null)
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El transportista no puede ser nulo",
                    Datos = null
                });
            }

            if (transportista.Id <= 0)
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El Id del transportista es invalido",
                    Datos = null
                });
            }

            if (string.IsNullOrEmpty(transportista.Nombre) || string.IsNullOrEmpty(transportista.Correo) || string.IsNullOrEmpty(transportista.Telefono))
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "Los campos Nombre, Correo y Telefono son obligatorios",
                    Datos = null
                });
            }

            var respuestaAPI = TransportistaService.ActualizarTransportista(transportista);

            return Ok(respuestaAPI);
        }

        [HttpDelete("EliminarTransportista")]
        public IActionResult EliminarTransportista(int id)
        {
            if (id <= 0)
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El Id del transportista es invalido",
                    Datos = null
                });
            }

            var respuestaAPI = TransportistaService.EliminarTransportista(id);

            return Ok(respuestaAPI);
        }
    }
}
