using EcomTrackAPI.Models;
using Microsoft.Data.SqlClient;

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
    }
}
