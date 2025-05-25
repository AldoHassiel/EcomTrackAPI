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

            if (!respuesta.Resultado)
            {
                return BadRequest(respuesta);
            }

            return Ok(respuesta);
        }

        [HttpPost("GuardarCategoria")]
        public IActionResult GuardarCategoria(Categoria categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria.Nombre))
            {
                return BadRequest(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El nombre es obligatorio",
                    Datos = null
                });
            }

            if (string.IsNullOrWhiteSpace(categoria.Descripcion))
            {
                return BadRequest(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "La descripción es obligatoria",
                    Datos = null
                });
            }
            var respuesta = CategoriaService.GuardarCategoria(categoria);

            if (!respuesta.Resultado)
            {
                return BadRequest(respuesta);
            }

            return Ok(respuesta);
        }

        [HttpPut("ActualizarCategoria")]
        public IActionResult ActualizarCategoria(Categoria categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria.Nombre))
            {
                return BadRequest(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El nombre es obligatorio",
                    Datos = null
                });
            }

            if (string.IsNullOrWhiteSpace(categoria.Descripcion))
            {
                return BadRequest(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "La descripción es obligatoria",
                    Datos = null
                });
            }

            var respuesta = CategoriaService.ActualizarCategoria(categoria);

            if (!respuesta.Resultado)
            {
                return BadRequest(respuesta);
            }

            return Ok(respuesta);
        }

        [HttpDelete("EliminarCategoria")]
        public IActionResult EliminarCategoria(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new RespuestaAPI
                {
                    Resultado = false,
                    Mensaje = "El ID de la categoría no es válido",
                    Datos = null
                });
            }

            var respuesta = CategoriaService.EliminarCategoria(id);

            if (!respuesta.Resultado)
            {
                return BadRequest(respuesta);
            }

            return Ok(respuesta);
        }
    }
}
