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
        [HttpGet("ObtenerProductos")]
        public IActionResult ObtenerProductos(int idCategoria, int idProveedor)
        {
            var respuestaAPI = ProductoService.ObtenerProductos(idCategoria, idProveedor);

            if (!respuestaAPI.Resultado)
            {
                return BadRequest(respuestaAPI);
            }

            return Ok(respuestaAPI);
        }
        [HttpPost("GuardarProducto")]
        public IActionResult GuardarProducto([FromBody] Producto producto)
        {
            if (producto == null)
            {
                return BadRequest(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El producto no puede ser nulo",
                    Datos = null
                });
            }

            var respuestaAPI = ProductoService.GuardarProducto(producto);

            if (!respuestaAPI.Resultado)
            {
                return BadRequest(respuestaAPI);
            }

            return Ok(respuestaAPI);
        }
        [HttpPut("ActualizarProducto")]
        public IActionResult ActualizarProducto([FromBody] Producto producto)
        {
            if (producto == null)
            {
                return BadRequest(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El producto no puede ser nulo",
                    Datos = null
                });
            }

            var respuestaAPI = ProductoService.ActualizarProducto(producto);

            if (!respuestaAPI.Resultado)
            {
                return BadRequest(respuestaAPI);
            }

            return Ok(respuestaAPI);
        }

        [HttpDelete("EliminarProducto")]
        public IActionResult EliminarProducto(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El ID del producto no puede ser menor o igual a cero",
                    Datos = null
                });
            }

            var respuestaAPI = ProductoService.EliminarProducto(id);

            if (!respuestaAPI.Resultado)
            {
                return BadRequest(respuestaAPI);
            }

            return Ok(respuestaAPI);
        }
    }
}