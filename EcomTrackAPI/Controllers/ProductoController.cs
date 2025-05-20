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
        public ActionResult<List<Producto>> ObtenerProductos()
        {
            try
            {
                List<Producto> productos = ProductoService.ObtenerProductos();
                if (productos == null || productos.Count == 0)
                {
                    return NotFound("No se encontraron productos.");
                }
                return Ok(productos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al obtener los productos: " + ex.Message);
            }
        }
        [HttpGet]
        public ActionResult<Producto> ObtenerProductoPorId(int id)
        {
            try
            {
                Producto producto = ProductoService.ObtenerProductoPorId(id);
                if (producto == null)
                {
                    return NotFound("Producto no encontrado.");
                }
                return Ok(producto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al obtener el producto: " + ex.Message);
            }
        }
    }
}