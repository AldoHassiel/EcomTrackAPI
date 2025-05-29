using EcomTrackAPI.Models;
using EcomTrackAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcomTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        [HttpGet("ObtenerCategorias")]
        public IActionResult ObtenerCategorias()
        {
            var respuesta = CategoriaService.ObtenerCategorias();
            return Ok(respuesta);
        }

        [HttpPost("GuardarCategoria")]
        public IActionResult GuardarCategoria([FromBody] Categoria categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria.Nombre))
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El nombre es obligatorio",
                    Datos = null
                });
            }

            if (string.IsNullOrWhiteSpace(categoria.Descripcion))
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "La descripción es obligatoria",
                    Datos = null
                });
            }

            var respuesta = CategoriaService.GuardarCategoria(categoria);

            return Ok(respuesta);
        }

        [HttpPut("ActualizarCategoria")]
        public IActionResult ActualizarCategoria([FromBody] Categoria categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria.Nombre))
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El nombre es obligatorio",
                    Datos = null
                });
            }

            if (string.IsNullOrWhiteSpace(categoria.Descripcion))
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "La descripción es obligatoria",
                    Datos = null
                });
            }

            var respuesta = CategoriaService.ActualizarCategoria(categoria);

            return Ok(respuesta);
        }

        [HttpDelete("EliminarCategoria")]
        public IActionResult EliminarCategoria(int id)
        {
            if (id <= 0)
            {
                return Ok(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El ID de la categoría no es válido",
                    Datos = null
                });
            }

            var respuesta = CategoriaService.EliminarCategoria(id);

            return Ok(respuesta);
        }

        [HttpGet("ObtenerIDyNombre")]
        public IActionResult ObtenerIDyNombre()
        {
            var respuesta = CategoriaService.ObtenerIDyNombre();
            return Ok(respuesta);
        }
    }
}
