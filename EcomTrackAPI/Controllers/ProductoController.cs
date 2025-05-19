using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcomTrackAPI.Models;
using EcomTrackAPI.Services;
namespace EcomTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        [HttpGet]
        public List<Producto> ObtenerProductos()
        {
            var productos = ProductoService.ObtenerProductos();
            return productos;
        }
    }
}