using EcomTrackAPI.Models;
using EcomTrackAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcomTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        [HttpGet]
        [Route("[action]")]
        public List<Categoria> ObtenerCategorias()
        {
            List<Categoria> categorias = CategoriaService.ObtenerCategorias();
            return categorias;
        }

        [HttpDelete("{id}")]
        public bool EliminarCategoria(int id) 
        {
            bool eliminado = CategoriaService.EliminarCategoria(id);
            return eliminado;
        }

        [HttpPost]
        [Route("[action]")]

        public IActionResult GuardarCategoria(Categoria categoria)
        {
            IActionResult guardado = CategoriaService.GuardarCategoria(categoria);
            return guardado;
        }

        public IActionResult ActualizarCategoria(Categoria categoria)
        {
            IActionResult actualizado = CategoriaService.ActualizarCategoria(categoria);
            return actualizado;
        }

    }
}
