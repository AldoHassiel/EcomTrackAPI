using EcomTrackAPI.Models;
using EcomTrackAPI.Models.DTO;
using EcomTrackAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcomTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoController : ControllerBase
    {
        [HttpGet("ObtenerCarrito")]
        public IActionResult ObtenerCarrito(int idCliente)
        {
            var respuesta = CarritoService.ObtenerCarrito(idCliente);
            return Ok(respuesta);
        }

        [HttpPost("AgregarProducto")]
        public IActionResult AgregarProductoAlCarrito([FromBody] AgregarProductoCarritoDTO dto)
        {

            if (dto.Cantidad <= 0)
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "La cantidad debe ser mayor a cero",
                    Datos = null
                });
            }

            var respuesta = CarritoService.AgregarProductoAlCarrito(dto);
            return Ok(respuesta);
        }

        [HttpPut("ActualizarCantidad")]
        public IActionResult ActualizarCantidadProducto([FromBody] ActualizarCantidadProductoDTO dto)
        {
            if (dto.Cantidad <= 0)
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "La cantidad debe ser mayor a cero",
                    Datos = null
                });
            }

            var respuesta = CarritoService.ActualizarCantidadProductoCarrito(dto);
            return Ok(respuesta);
        }

        [HttpDelete("EliminarProducto")]
        public IActionResult EliminarProductoDelCarrito([FromBody] EliminarProductoCarritoDTO dto)
        {
            var respuesta = CarritoService.EliminarProductoDelCarrito(dto);
            return Ok(respuesta);
        }

        [HttpPost("ConfirmarCompra")]
        public IActionResult ConfirmarCompra([FromBody] ConfirmarCompraDTO dto)
        {
            var respuesta = CarritoService.ConfirmarCompra(dto);
            return Ok(respuesta);
        }
    }
}
