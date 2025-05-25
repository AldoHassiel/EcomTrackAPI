using EcomTrackAPI.Models;
using EcomTrackAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcomTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        [HttpGet("ObtenerPedidos")]
        
        public IActionResult ObtenerPedidos()
        {
            var respuesta = PedidosService.ObtenerPedidos();

            if (!respuesta.Resultado)
            {
                return BadRequest(respuesta);
            }

            return Ok(respuesta);
        }

        [HttpPost("RegistrarPedido")]

        public IActionResult RegistrarPedido(Pedidos pedido)
        {
            var respuesta = PedidosService.RegistrarPedido(pedido);

            if (!respuesta.Resultado)
            {
                return BadRequest(respuesta);
            }

            return Ok(respuesta);

        }

        [HttpPost("ActualizarEstadoPedido")]
        public IActionResult ActualizarEstadoPedido(Pedidos pedido)
        {
            var respuesta = PedidosService.ActualizarEstadoPedido(pedido);

            if (!respuesta.Resultado)
            {
                return BadRequest(respuesta);
            }

            return Ok(respuesta);
        }

    }
}
