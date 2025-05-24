using EcomTrackAPI.Models;
using EcomTrackAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcomTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        [HttpGet]
        [Route("[action]")]
        public List<Pedidos> ObtenerPedidos()
        {
            List<Pedidos> pedidos = PedidosService.ObtenerPedidos();
            return pedidos;
        }

        [HttpPost]
        [Route("[action]")]

        public IActionResult RegistrarPedido(Pedidos pedido)
        {
            IActionResult guardar = PedidosService.RegistrarPedido(pedido);
            return guardar;
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult ActualizarEstadoPedido(Pedidos pedido)
        {
            IActionResult guardar = PedidosService.ActualizarEstadoPedido(pedido);
            return guardar;
        }

    }
}
