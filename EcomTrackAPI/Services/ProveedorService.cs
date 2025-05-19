using EcomTrackAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EcomTrackAPI.Services
{
    public class ProveedorService
    {
        public static List<Proveedor> ObtenerProveedores()
        {
            List<Proveedor> proveedores = new List<Proveedor>();
            SqlConnection conx = Datos.conectar();

            string query = "select Id, Nombre, Correo, Telefono, Estado from Proveedor";
            SqlCommand command = new SqlCommand(query, conx);

            conx.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Proveedor proveedor = new Proveedor()
                {
                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Correo = reader.GetString(2),
                    Telefono = reader.GetString(3),
                    Estado = reader.GetBoolean(4)
                };
                proveedores.Add(proveedor);
            }

            reader.Close();
            command.Dispose();
            conx.Close();

            return proveedores;
        }

        public static bool EliminarProveedor(int id)
        {
            SqlConnection conx = Datos.conectar();
            string query = "delete from Proveedor where Id = @Id";

            SqlCommand command = new SqlCommand(query, conx);
            command.Parameters.AddWithValue("@Id", id);

            conx.Open();
            int filasAfectadas = command.ExecuteNonQuery();
            conx.Close();

            return filasAfectadas > 0;
        }

        public static IActionResult GuardarProveedor(Proveedor proveedor)
        {
            SqlConnection conx = Datos.conectar();
            SqlCommand cmd = new SqlCommand("GuardarProveedor", conx);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", proveedor.Id);
            cmd.Parameters.AddWithValue("@Nombre", proveedor.Nombre);
            cmd.Parameters.AddWithValue("@Descripcion", proveedor.Correo);
            cmd.Parameters.AddWithValue("@CantidadProducto", proveedor.Telefono);
            cmd.Parameters.AddWithValue("@Estado", proveedor.Estado);

            conx.Open();
            try
            {
                cmd.ExecuteNonQuery();
                return new OkObjectResult("Proveedor guardado");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error al guardar: {ex.Message}");
            }
            finally { conx.Close(); }
        }
    }
}
