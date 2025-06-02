using EcomTrackAPI.Models;
using EcomTrackAPI.Services;
using Microsoft.AspNetCore.Mvc;


namespace EcomTrackAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class TipoDireccionController : ControllerBase
    {
        [HttpGet("ObtenerTipoDireccion")]
        public IActionResult ObtenerTipoDireccion()
        {
            var respuesta = TipoDireccionService.ObtenerTipoDireccion();
            return Ok(respuesta);
        }

    }
}
