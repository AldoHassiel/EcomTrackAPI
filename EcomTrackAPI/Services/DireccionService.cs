using Microsoft.Data.SqlClient;
using EcomTrackAPI.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace EcomTrackAPI.Services
{
    public class DireccionService
    {
        public static List<Direccion> ListaDirecciones()
        {
            List<Direccion> direcciones = new List<Direccion>();
            SqlConnection conx = Datos.conectar();

            string query = "select Id, Cliente, Tipo, Calle, EntreCalles, Numero, Colonia, " +
                "Ciudad, Estado, CodigoPostal, Pais from Direccion";
            SqlCommand command = new SqlCommand(query, conx);

            conx.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Direccion direccion = new Direccion()
                {
                    Id = reader.GetInt32(0),
                    Cliente = reader.GetInt32(1),
                    Tipo = reader.GetInt32(2),
                    Calle = reader.GetString(3),
                    EntreCalles = reader.GetString(4),
                    Numero = reader.GetString(5),
                    Colonia = reader.GetString(6),
                    Ciudad = reader.GetString(7),
                    Estado = reader.GetString(8),
                    CodigoPostal = reader.GetString(9),
                    Pais = reader.GetString(10),
                };
                direcciones.Add(direccion);
            }

            reader.Close();
            command.Dispose();
            conx.Close();

            return direcciones;
        }
        public static IActionResult EditarDireccion(Direccion direccion)
        {
            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("ActualizarDireccion", conx);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", direccion.Id);
            cmd.Parameters.AddWithValue("@Cliente", direccion.Cliente);
            cmd.Parameters.AddWithValue("@Tipo", direccion.Tipo);
            cmd.Parameters.AddWithValue("@Calle", direccion.Calle);
            cmd.Parameters.AddWithValue("@EntreCalles", direccion.EntreCalles);
            cmd.Parameters.AddWithValue("@Numero", direccion.Numero);
            cmd.Parameters.AddWithValue("@Colonia", direccion.Colonia);
            cmd.Parameters.AddWithValue("@Ciudad", direccion.Ciudad);
            cmd.Parameters.AddWithValue("@Estado", direccion.Estado);
            cmd.Parameters.AddWithValue("@CodigoPostal", direccion.CodigoPostal);
            cmd.Parameters.AddWithValue("@Pais", direccion.Pais);


            conx.Open();
            try
            {
                cmd.ExecuteNonQuery();
                return new OkObjectResult("Direccion actualizada");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error al actualizar la direccion: {ex.Message}");
            }
            finally { conx.Close(); }
        }
        public static IActionResult GuardarDireccion(Direccion direccion)
        {
            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("GuardarDireccion", conx);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", direccion.Id);
            cmd.Parameters.AddWithValue("@Cliente", direccion.Cliente);
            cmd.Parameters.AddWithValue("@Tipo", direccion.Tipo);
            cmd.Parameters.AddWithValue("@Calle", direccion.Calle);
            cmd.Parameters.AddWithValue("@EntreCalles", direccion.EntreCalles);
            cmd.Parameters.AddWithValue("@Numero", direccion.Numero);
            cmd.Parameters.AddWithValue("@Colonia", direccion.Colonia);
            cmd.Parameters.AddWithValue("@Ciudad", direccion.Ciudad);
            cmd.Parameters.AddWithValue("@Estado", direccion.Estado);
            cmd.Parameters.AddWithValue("@CodigoPostal", direccion.CodigoPostal);
            cmd.Parameters.AddWithValue("@Pais", direccion.Pais);


            conx.Open();
            try
            {
                cmd.ExecuteNonQuery();
                return new OkObjectResult("Direccion guardada");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error al guardar la direccion: {ex.Message}");
            }
            finally { conx.Close(); }
        }
        public static bool EliminarDireccion(int id)
        {
            SqlConnection conx = Datos.conectar();
            string query = "delete from Direccion where Id = @Id";

            SqlCommand command = new SqlCommand(query, conx);
            command.Parameters.AddWithValue("@Id", id);

            conx.Open();
            int filasAfectadas = command.ExecuteNonQuery();
            conx.Close();

            return filasAfectadas > 0;
        }
    }
}
