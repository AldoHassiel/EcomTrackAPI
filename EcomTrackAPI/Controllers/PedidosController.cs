using EcomTrackAPI.Models;
using EcomTrackAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcomTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        [HttpGet("ObtenerPedidosDelCliente")]
        public IActionResult ObtenerPedidosCliente(int IdCliente)
        {
            var respuesta = PedidosService.ObtenerPedidosDelCliente(IdCliente);
            return Ok(respuesta);
        }
        [HttpGet("ObtenerDetallePedidoCliente")]
        public IActionResult ObtenerDetallePedidoCliente(int IdCliente, int IdPedido)
        {
            var respuesta = PedidosService.ObtenerDetallePedidoCliente(IdCliente, IdPedido);
            return Ok(respuesta);
        }

        [HttpGet("ObtenerIngresosTotales")]
        public IActionResult ObtenerIngresosTotales()
        {
            var respuesta = PedidosService.ObtenerIngresosTotales();
            return Ok(respuesta);
        }
    }
}
