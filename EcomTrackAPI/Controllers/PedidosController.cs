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
        [HttpGet("ObtenerPedidosPeriodo")]
        public IActionResult ObtenerPedidosPeriodo(int FechaInicio = 0, int FechaFin = 0, int Estado = 0)
        {
            var respuesta = PedidosService.ObtenerPedidosPeriodo(FechaInicio, FechaFin, Estado);
            return Ok(respuesta);
        }
    }
}
