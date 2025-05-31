using EcomTrackAPI.Models;
using EcomTrackAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcomTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnvioController : ControllerBase
    {
        [HttpGet("ObtenerEnvios")]
        public IActionResult ObtenerEnvios()
        {
            var respuestaAPI = EnvioService.ObtenerEnvios();

            return Ok(respuestaAPI);
        }

        [HttpPost("GuardarEnvio")]
        public IActionResult GuardarEnvio([FromBody] Envio envio)
        {
            if (envio == null)
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El envio no puede ser nulo",
                    Datos = null
                });
            }

            var respuestaAPI = EnvioService.GuardarEnvio(envio);

            return Ok(respuestaAPI);
        }

        [HttpPut("ActualizarEnvio")]
        public IActionResult ActualizarEnvio([FromBody] Envio envio)
        {
            if (envio == null)
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El envio no puede ser nulo",
                    Datos = null
                });
            }

            if (envio.Id <= 0)
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El Id del envio es inválido",
                    Datos = null
                });
            }

            var respuestaAPI = EnvioService.ActualizarEnvio(envio);

            return Ok(respuestaAPI);
        }


    }
}
