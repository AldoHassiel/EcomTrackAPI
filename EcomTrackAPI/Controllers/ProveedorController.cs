using EcomTrackAPI.Models;
using EcomTrackAPI.Services;
using Microsoft.AspNetCore.Mvc;


namespace EcomTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedorController : ControllerBase
    {
        [HttpGet("ObtenerProveedores")]
        public IActionResult ObtenerProveedores()
        {
            var respuestaAPI = ProveedorService.ObtenerProveedores();

            if (!respuestaAPI.Resultado)
            {
                return BadRequest(respuestaAPI);
            }

            return Ok(respuestaAPI);
        }

        [HttpPost("GuardarProveedor")]
        public IActionResult GuardarProveedor([FromBody] Proveedor proveedor)
        {
            if (proveedor == null)
            {
                return BadRequest(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El proveedor no puede ser nulo",
                    Datos = null
                });
            }

            if (string.IsNullOrEmpty(proveedor.Nombre) || string.IsNullOrEmpty(proveedor.Correo) || string.IsNullOrEmpty(proveedor.Telefono))
            {
                return BadRequest(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "Los campos Nombre, Correo y Telefono son obligatorios",
                    Datos = null
                });
            }

            var respuestaAPI = ProveedorService.GuardarProveedor(proveedor);

            if (!respuestaAPI.Resultado)
            {
                return BadRequest(respuestaAPI);
            }

            return Ok(respuestaAPI);
        }

        [HttpPut("ActualizarProveedor")]
        public IActionResult ActualizarProveedor([FromBody] Proveedor proveedor)
        {
            if (proveedor == null)
            {
                return BadRequest(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El proveedor no puede ser nulo",
                    Datos = null
                });
            }
            
            if (proveedor.Id <= 0)
            {
                return BadRequest(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El Id del proveedor es invalido",
                    Datos = null
                });
            }

            if (string.IsNullOrEmpty(proveedor.Nombre) || string.IsNullOrEmpty(proveedor.Correo) || string.IsNullOrEmpty(proveedor.Telefono))
            {
                return BadRequest(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "Los campos Nombre, Correo y Telefono son obligatorios",
                    Datos = null
                });
            }

            var respuestaAPI = ProveedorService.ActualizarProveedor(proveedor);

            if (!respuestaAPI.Resultado)
            {
                return BadRequest(respuestaAPI);
            }

            return Ok(respuestaAPI);
        }

        [HttpDelete("EliminarProveedor/")]
        public IActionResult EliminarProveedor(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El Id del proveedor es invalido",
                    Datos = null
                });
            }

            var respuestaAPI = ProveedorService.EliminarProveedor(id);

            if (!respuestaAPI.Resultado)
            {
                return BadRequest(respuestaAPI);
            }

            return Ok(respuestaAPI);
        }
    }
}