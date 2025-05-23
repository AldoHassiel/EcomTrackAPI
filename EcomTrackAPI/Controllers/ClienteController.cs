using EcomTrackAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcomTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        [HttpPost("RegistrarCliente")]
        public IActionResult RegistrarCliente([FromBody] ClienteRegistro cliente)
        {
            if (cliente == null) return BadRequest("El cliente no puede ser nulo.");

            var resultado = Services.ClienteService.RegistrarCliente(cliente);

            if (resultado.Resultado)
            {
                return Ok(new { Resultado = true, Mensaje = "Cliente registrado correctamente", IdCliente = resultado.IdUsuario });
            }

            return BadRequest(new { Resultado = false, Mensaje = resultado.Mensaje });
        }

        [HttpPut("ActualizarCliente")]
        public IActionResult ActualizarCliente([FromBody] ClienteActualizar cliente)
        {
            if (cliente == null) return BadRequest("El cliente no puede ser nulo.");

            var resultado = Services.ClienteService.ActualizarCliente(cliente);

            if (resultado.Resultado)
            {
                return Ok(new { Resultado = true, Mensaje = "Cliente actualizado correctamente" });
            }
            return BadRequest(new { Resultado = false, Mensaje = resultado.Mensaje });
        }

        [HttpDelete("EliminarCliente")]
        public IActionResult EliminarCliente(int id)
        {
            if (id <= 0) return BadRequest("El ID del cliente no es válido.");

            var resultado = Services.ClienteService.EliminarCliente(id);

            if (resultado.Resultado)
            {
                return Ok(new { Resultado = true, Mensaje = "Cliente eliminado correctamente" });
            }
            return BadRequest(new { Resultado = false, Mensaje = resultado.Mensaje });
        }

        [HttpGet("ObtenerClientePorId")]
        public IActionResult ObtenerClientePorId(int id)
        {
            if (id <= 0) return BadRequest("El ID del cliente no es válido.");

            var cliente = Services.ClienteService.ObtenerClientePorId(id);

            if (cliente == null)
            {
                return NotFound(new { Mensaje = "Cliente no encontrado" });
            }

            return Ok(new { Resultado = true, cliente });
        }

        [HttpGet("ObtenerClientes")]
        public IActionResult ObtenerClientes([FromQuery] int orden = 0)
        {
            var clientes = Services.ClienteService.ObtenerClientes(orden);

            if (clientes == null || clientes.Count == 0)
            {
                return NotFound(new { Resultado = false, Mensaje = "No se encontraron clientes" });
            }
            return Ok( new { Resultado = true , clientes });
        }
    }
}
