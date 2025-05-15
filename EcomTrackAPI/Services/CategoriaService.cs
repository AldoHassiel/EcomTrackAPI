using Microsoft.Data.SqlClient;
using EcomTrackAPI.Models;

namespace EcomTrackAPI.Services
{
    public class CategoriaService
    {
        public static List<Categoria> ObtenerCategorias()
        {
            List<Categoria> categorias = new List<Categoria>();
            SqlConnection conx = Datos.conectar();

            string query = "select Id, Nombre, Descripcion, CantidadProducto, Estado from Categoria";
            SqlCommand command = new SqlCommand(query, conx);

            conx.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Categoria categoria = new Categoria()
                {
                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Descripcion = reader.GetString(2),
                    CantidadProducto = reader.GetInt32(3),
                    Estado = reader.GetBoolean(4)
                };
                categorias.Add(categoria);
            }

            reader.Close();
            command.Dispose();
            conx.Close();

            return categorias;
        }
    }
}
