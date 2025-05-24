using EcomTrackAPI.Models;
using EcomTrackAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcomTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PagoController : ControllerBase
    {
        [HttpGet]
        [Route("[action]")]
        public List<Pago> ListaDePagos()
        {
            List<Pago> pagos = PagoService.ListaDePagos();
            return pagos;
        }

        [HttpPost]
        [Route("[action]")]

        public IActionResult RealizarPago(Pago pago)
        {
            IActionResult guardar = PagoService.RealizarPago(pago);
            return guardar;
        }
    }
}
