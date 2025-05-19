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
        public bool EliminarCategoria(int id) //pendiente
        {
            bool eliminado = CategoriaService.EliminarCategoria(id);
            return eliminado;
        }

        [HttpPost]
        [Route("[action]")]

        public void GuardarCategoria(Categoria categoria)
        {
           
      
        }


    }
}
