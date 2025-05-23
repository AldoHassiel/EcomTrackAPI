using EcomTrackAPI.Models;
using EcomTrackAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcomTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnvioController : ControllerBase
    {
        [HttpGet]
        [Route("[action]")]
        public List<Envio> ObtenerEnvio()
        {
            List<Envio> envios = EnvioService.ObtenerEnvios();
            return envios;
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult GuardarEnvio(Envio envio)
        {
            IActionResult guardado = EnvioService.GuardarEnvio(envio);
            return guardado;
        }
    }
}
