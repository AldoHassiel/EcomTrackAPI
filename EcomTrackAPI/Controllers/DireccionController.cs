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
        [HttpPost("AgregarDireccion")]
        public IActionResult AgregarDireccion([FromBody] DireccionAgregarDTO direccion)
        {
            var respuesta = DireccionesService.AgregarDireccion(direccion);
            return Ok(respuesta);
        }
        [HttpPut("EditarDireccion")]
        public IActionResult EditarDireccion([FromBody] DireccionEditarDTO direccion)
        {
            var respuesta = DireccionesService.EditarDireccion(direccion);
            return Ok(respuesta);
        }
        [HttpDelete("EliminarDireccion")]
        public IActionResult EliminarDireccion(int id, int idCliente)
        {
            var respuesta = DireccionesService.EliminarDireccion(id, idCliente);
            return Ok(respuesta);
        }
    }
}
