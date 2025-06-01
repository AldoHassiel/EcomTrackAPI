using EcomTrackAPI.Models;
using EcomTrackAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcomTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarjetaController : ControllerBase
    {
        [HttpGet("ObtenerTarjetas")]
        public IActionResult ObtenerTarjetas(int id)
        {
            var respuesta = TarjetaService.ObtenerTarjetas(id);
            return Ok(respuesta);
        }

        [HttpPost("GuardarTarjeta")]
        public IActionResult GuardarTarjeta([FromBody] Tarjeta tarjeta)
        {
            if (string.IsNullOrWhiteSpace(tarjeta.NumeroTarjeta))
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El numero de la tarjeta es obligatorio",
                    Datos = null
                });
            }

            var respuesta = TarjetaService.GuardarTarjeta(tarjeta);

            return Ok(respuesta);
        }

        [HttpPut("ActualizarTarjeta")]
        public IActionResult ActualizarTarjeta([FromBody] int id)
        {
            

            var respuesta = TarjetaService.ActualizarTarjeta(id);

            return Ok(respuesta);
        }

        [HttpDelete("EliminarTarjeta")]
        public IActionResult EliminarTarjeta(int id)
        {
            if (id <= 0)
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El ID de la tarjeta no es válido",
                    Datos = null
                });
            }

            var respuesta = TarjetaService.EliminarTarjeta(id);

            return Ok(respuesta);
        }

    }
}
