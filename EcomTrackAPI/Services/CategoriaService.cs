using Microsoft.Data.SqlClient;
using EcomTrackAPI.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc;

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
        public static bool EliminarCategoria(int id)
        {
            SqlConnection conx = Datos.conectar();
            string query = "delete from Categoria where Id = @Id";

            SqlCommand command = new SqlCommand(query, conx);
            command.Parameters.AddWithValue("@Id", id);

            conx.Open();
            int filasAfectadas = command.ExecuteNonQuery();
            conx.Close();

            return filasAfectadas > 0;
        }

        public static IActionResult GuardarCategoria(Categoria categoria)
        {
            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("GuardarCategoria", conx);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", categoria.Id);
            cmd.Parameters.AddWithValue("@Nombre", categoria.Nombre);
            cmd.Parameters.AddWithValue("@Descripcion", categoria.Descripcion);
            cmd.Parameters.AddWithValue("@CantidadProducto", categoria.CantidadProducto);
            cmd.Parameters.AddWithValue("@Estado", categoria.Estado);

            conx.Open();
            try
            {
                cmd.ExecuteNonQuery();
                return new OkObjectResult("Categoría guardada");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error al guardar: {ex.Message}");
            }
            finally { conx.Close(); }
        }

        public static IActionResult ActualizarCategoria(Categoria categoria)
        {
            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("ActualizarCategoria", conx);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", categoria.Id);
            cmd.Parameters.AddWithValue("@Nombre", categoria.Nombre);
            cmd.Parameters.AddWithValue("@Descripcion", categoria.Descripcion);
            cmd.Parameters.AddWithValue("@CantidadProducto", categoria.CantidadProducto);
            cmd.Parameters.AddWithValue("@Estado", categoria.Estado);

            conx.Open();
            try
            {
                cmd.ExecuteNonQuery();
                return new OkObjectResult("Categoría actualizada");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error al actualizar: {ex.Message}");
            }
            finally { conx.Close(); }
        }
    }
}
