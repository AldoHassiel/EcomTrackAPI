using EcomTrackAPI.Models;
using EcomTrackAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcomTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DireccionController : ControllerBase
    {
        [HttpGet]
        [Route("[action]")]
        public List<Direccion> ListaDirecciones()
        {
            List<Direccion> direcciones = DireccionService.ListaDirecciones();
            return direcciones;
        }

        [HttpDelete("{id}")]
        public bool Eliminardireccion(int id)
        {
            bool eliminado = DireccionService.EliminarDireccion(id);
            return eliminado;
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult GuardarDireccion(Direccion direccion)
        {
            IActionResult guardado = DireccionService.GuardarDireccion(direccion);
            return guardado;
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult EditarDireccion(Direccion direccion)
        {
            IActionResult guardado = DireccionService.EditarDireccion(direccion);
            return guardado;
        }
    }
}
