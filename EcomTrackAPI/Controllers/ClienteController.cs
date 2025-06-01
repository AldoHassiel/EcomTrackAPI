using EcomTrackAPI.Models;
using EcomTrackAPI.Services;
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
            if (cliente == null)
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El cliente no puede ser nulo",
                    Datos = null
                });
            }

            if (string.IsNullOrWhiteSpace(cliente.Correo))
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El correo es obligatorio",
                    Datos = null
                });
            }

            if (string.IsNullOrWhiteSpace(cliente.Pwd))
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "La contraseña es obligatoria",
                    Datos = null
                });
            }

            if (string.IsNullOrWhiteSpace(cliente.Nombre))
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El nombre es obligatorio",
                    Datos = null
                });
            }

            if (string.IsNullOrWhiteSpace(cliente.Telefono))
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El teléfono es obligatorio",
                    Datos = null
                });
            }

            var respuesta = ClienteService.RegistrarCliente(cliente);


            return Ok(respuesta);
        }

        [HttpPut("ActualizarCliente")]
        public IActionResult ActualizarCliente([FromBody] ClienteActualizar cliente)
        {
            if (cliente == null)
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El cliente no puede ser nulo",
                    Datos = null
                });
            }
            if (string.IsNullOrWhiteSpace(cliente.Correo))
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El correo es obligatorio",
                    Datos = null
                });
            }
            if (string.IsNullOrWhiteSpace(cliente.Pwd))
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "La contraseña es obligatoria",
                    Datos = null
                });
            }
            if (string.IsNullOrWhiteSpace(cliente.Nombre))
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El nombre es obligatorio",
                    Datos = null
                });
            }
            if (string.IsNullOrWhiteSpace(cliente.Telefono))
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El teléfono es obligatorio",
                    Datos = null
                });
            }

            var respuesta = ClienteService.ActualizarCliente(cliente);

            return Ok(respuesta);
        }

        [HttpDelete("EliminarCliente")]
        public IActionResult EliminarCliente(int id)
        {
            if (id <= 0)
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El ID del cliente es inválido",
                    Datos = null
                });
            }
            var respuesta = ClienteService.EliminarCliente(id);
            return Ok(respuesta);
        }

        [HttpGet("ObtenerUnCliente")]
        public IActionResult ObtenerUnCliente(int id)
        {
            if (id <= 0)
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El ID del cliente es inválido",
                    Datos = null
                });
            }
            var respuesta = ClienteService.ObtenerUnCliente(id);
            return Ok(respuesta);
        }

        [HttpGet("ObtenerClientes")]
        public IActionResult ObtenerClientes(int orden)
        {
            var respuesta = ClienteService.ObtenerClientes(orden);
            return Ok(respuesta);
        }

        [HttpGet("top-clientes")]
        public IActionResult ObtenerTopClientes()
        {
            var res = ClienteService.ObtenerTopClientes();
            return Ok(res);
        }
    }
}
