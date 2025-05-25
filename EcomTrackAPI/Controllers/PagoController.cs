using EcomTrackAPI.Models;
using EcomTrackAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcomTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PagoController : ControllerBase
    {
        [HttpGet("ListaDePagos")]
        public IActionResult ListaDePagos()
        {
            var respuesta = PagoService.ListaDePagos();

            if (!respuesta.Resultado)
            {
                return BadRequest(respuesta);
            }

            return Ok(respuesta);
        }

        [HttpPost("RealizarPago")]

        public IActionResult RealizarPago(Pago pago)
        {
            var respuesta = PagoService.RealizarPago(pago);

            if (!respuesta.Resultado)
            {
                return BadRequest(respuesta);
            }

            return Ok(respuesta);
        }
    }
}
