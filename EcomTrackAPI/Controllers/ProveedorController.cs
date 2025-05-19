using EcomTrackAPI.Models;
using EcomTrackAPI.Services;
using Microsoft.AspNetCore.Mvc;


namespace EcomTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedorController : ControllerBase
    {
        [HttpGet]
        [Route("[action]")]
        public List<Proveedor> ObtenerProveedores()
        {
            List<Proveedor> proveedores = ProveedorService.ObtenerProveedores();
            return proveedores;
        }

        [HttpDelete("{id}")]
        public bool EliminarProveedor(int id) //pendiente
        {
            bool eliminado = ProveedorService.EliminarProveedor(id);
            return eliminado;
        }
    }
    
}
