using EcomTrackAPI.Models;
using EcomTrackAPI.Models.DTO;
using EcomTrackAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcomTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DireccionController : ControllerBase
    {
        [HttpGet("ObtenerDirecciones")]
        public IActionResult ObtenerDirecciones(int cliente)
        {
            var respuesta = DireccionesService.ObtenerDirecciones(cliente);
            return Ok(respuesta);
        }
    }
}
