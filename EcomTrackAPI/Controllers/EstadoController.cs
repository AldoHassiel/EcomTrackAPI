using EcomTrackAPI.Models;
using EcomTrackAPI.Services;
using Microsoft.AspNetCore.Mvc;


namespace EcomTrackAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class EstadoController : ControllerBase
    {
        [HttpGet("ObtenerEstado")]
        public IActionResult ObtenerEstado()
        {
            var respuesta = EstadoService.ObtenerEstado();
            return Ok(respuesta);
        }

    }
}