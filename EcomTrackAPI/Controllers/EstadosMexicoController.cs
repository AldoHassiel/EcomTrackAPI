using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcomTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadosMexicoController : ControllerBase
    {
        [HttpGet("ObtenerEstados")]
        public IActionResult ObtenerEstados()
        {
            var respuesta = Services.EstadoMexicoService.ObtenerEstados();
            return Ok(respuesta);
        }
    }
}
